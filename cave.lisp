#!/usr/bin/clisp
;Authors: James, Michael, Vladimir
;Purpose: Find the fastest exit out of a cave system using breadth-first
;Copyright 2011 James Koval, Michael, and Vladimir

;Nodes are kept in a hash table
(defparameter caves (make-hash-table))

;Bread crumb trail is in a hash table of how to return to start
;Keys are children and values are parents
;The values are caves that are closer to the start
(defparameter parents (make-hash-table))

(defun init ()
"Initialize the caves"
  (cave 'start :children '(e a c b) :costs '(2) :att 'start)
  (cave 'e :children '(start h a g) :costs '(2 2))
  (cave 'a :children '(start f e))
  (cave 'c :children '(start n d))
  (cave 'b :children '(start j))
  (cave 'h :children '(e) :costs '(2))
  (cave 'g :children '(troll-gi))
  (cave 'f :children '(a pit-fil))
  (cave 'n :children '(pit-jiln troll-pit))
  (cave 'd :children '(food-pit))
  (cave 'j :children '(food-start k pit-jiln))
  (cave 'troll-gi :children '(g i) :att 'troll)
  (cave 'pit-fil :children '(f i l) :att 'pit)
  (cave 'pit-jiln :children '(i l j n) :costs '(2 2) :att 'pit)
  (cave 'troll-pit :children '(pit-jiln n) :att 'troll)
  (cave 'food-pit :children '(pit-treasure) :att 'food)
  (cave 'food-start :children '(start f k) :att 'food)
  (cave 'k :children '(food-start j))
  (cave 'i :children '(pit-jiln pit-fil treasure) :costs '(2))
  (cave 'l :children '(pit-jiln pit-fil m) :costs '(2))
  (cave 'pit-treasure :children '(treasure) :att 'pit)
  (cave 'treasure :att 'end)
  (cave 'm :children '(l treasure))
)

(defun cave (name &key children (att 'harmless) costs)
"A cave is made of a name: hash key, mapped to a value of this format:
   (children-list of connected cave names)
     start/end/troll/food/pit/harmless-enum    explored/unexplored-enum
     costs-parallel to children list, containing costs that aren't 1
 This function uses optional parameters: children att costs
 att has the default value of harmless
"
  (setf (gethash name caves) (list children att costs)))

(defun get-info (name)
"Get the info for a cave"
  (gethash name caves))

(defun get-children (name)
"return all children of a cave"
  (first (get-info name)))

(defun get-cost (parent child)
"return the cost to travel from parent to child
  returns 1 if the cost isn't marked"
  (setf costs (third (get-info parent)))
  (unless costs (return-from get-cost 1))
  (setf children (get-children parent))
  (setf i 0)
  (setf found nil)
  (loop for name in children do
    (when (equal name child) (setf found t) (return nil))
    (incf i))
  (unless found (return-from get-cost 1))
  (unless (nth i costs) (return-from get-cost 1))
  (nth i costs))

(defun get-att (name)
"Get the attribute of a cave"
  (second (get-info name)))

(defun is-end (name)
"Return true if this is an end cave, otherwise false"
  (equal (get-att name) 'end))

(defun is-start (name)
"Return true if this is a start cave, otherwise false"
  (equal (get-att name) 'start))

(defun has-troll (name)
"Return true if this is a troll cave, otherwise false"
  (equal (get-att name) 'troll))

(defun has-food (name)
"Return true if this is a food cave, otherwise false"
  (equal (get-att name) 'food))

(defun has-pit (name)
"Return true if this is a pit, otherwise false"
  (equal (get-att name) 'pit))

(defun print-caves (&rest names)
"Print information about each requested cave"
  (loop for name in names do
    (print name)
    (print (get-info name))))

(defun print-all-caves ()
"Print information about all caves"
  (loop for key being the hash-keys of caves do
    (print-caves key)))

(defun get-start ()
"Find and return a start cave"
  (loop for name being the hash-keys of caves do
    (when (equal (second (get-info name)) 'start)
      (return-from get-start name))))

(defun get-food-caves()
"Find and return names of all the food caves"
  (setf foods nil)
  (loop for name being the hash-keys of caves do
    (when (has-food name) (push name foods)))
  foods)

;TODO: Food is infinite when nil, target is a cave other than end when set
(defun depth (name &optional food (steps 0) &key target)
  (print (list name food steps))
  (when (is-end name) (return-from depth (list name)))
  (when (has-troll name) (return-from depth nil))
  (when (< food 0) (return-from depth nil))
  (when (> steps 14) (return-from depth nil))
  ;not taking more than 15 steps is another hack to limit the search domain,
  ; such that the algorithm can finish within a second

  (when (has-pit name) (decf food 5))
  (when (has-food name) (incf food 5))
  (setf children (get-children name))
  (loop for child in children do
    (setf answer (depth child (- food (get-cost name child)) (+ steps 1)))
    (when (and answer (> food 0)) (return-from depth (append answer (list name)))))
          ;requiring more than 2 food is a hack specific to Tom's big cave,
          ;to force the algorithm to find the infinite food loop before exiting
  nil)

(defun main ()
  ;Initialize cave system - the caves hashtable
  (init)

  (setf initial-food 5)

  ;Get the food caves that have loops back to themselves
  ;These loops must produce more food than they comsume
  (setf foods (get-food-caves))
  (setf loop-foods nil)
  (loop for name in foods do
    (when (depth name initial-food :target name) (push name loop-foods)))

  ;Find path from start to food
  (defparameter food-paths (make-hash-table))
  (loop for name in loop-foods do
    (when (setf path (depth (get-start) initial-food :target name))
      (push path (gethash name food-paths))))

  ;Find path from food cave to end. Make this breadth first, or implement explored caves
  (setf successful-caves nil)
  (loop for name being the hash-keys of food-paths do
    (when (setf path (depth name))
      (push path (gethash name food-paths))
      (push name successful-caves)))

  (when successful-caves
    (print "Found infinite food paths")
    (loop for name in successful-caves do
      (print (gethash name food-paths))))

  ;(print (depth (get-start) 5 0))

  ;wait for user input at the end rather than auto-quiting
  ;(read-line)
)

(main)
