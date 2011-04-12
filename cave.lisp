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
  (cave 'start :children '(e a c b) :att 'start)
  (cave 'e :children '(start h a g))
  (cave 'a :children '(start f e))
  (cave 'c :children '(start n d))
  (cave 'b :children '(start j))
  (cave 'h :children '(e))
  (cave 'g :children '(troll-gi))
  (cave 'f :children '(a pit-fil))
  (cave 'n :children '(pit-jiln troll-pit))
  (cave 'd :children '(food-pit))
  (cave 'j :children '(food-start k pit-jiln))
  (cave 'troll-gi :children '(g i) :att 'troll)
  (cave 'pit-fil :children '(f i l) :att 'pit)
  (cave 'pit-jiln :children '(j i l n) :att 'pit)
  (cave 'troll-pit :children '(pit-jiln n) :att 'troll)
  (cave 'food-pit :children '(pit-treasure) :att 'food)
  (cave 'food-start :children '(start f k) :att 'food)
  (cave 'k :children '(food-start j))
  (cave 'i :children '(pit-fil treasure pit-jiln))
  (cave 'l :children '(pit-fil pit-jiln m))
  (cave 'pit-treasure :children '(treasure) :att 'pit)
  (cave 'treasure :att 'end)
  (cave 'm :children '(l treasure))
)

(defun cave (name &key children (att 'harmless))
"A cave is made of a name: hash key, mapped to a value of this format:
   (children-list of connected cave names)
     start/end/troll/food/pit/harmless-enum    explored/unexplored-enum
 This function uses optional parameters: children and att
 att has the default value of harmless
"
  (setf (gethash name caves) (list children att 'unexplored)))

(defun get-info (name)
"Get the info for a cave"
  (gethash name caves))

(defun get-children (name)
"return all children of a cave"
  (first (get-info name)))

(defun get-att (name)
"Get the attribute of a cave"
  (second (get-info name)))

(defun is-end (name)
"Return true if this is an end cave, otherwise false"
  (equal (get-att name) 'end))

(defun is-start (name)
"Return true if this is a start cave, otherwise false"
  (equal (get-att name) 'start))

(defun is-troll-free (name)
"Return true if this is a troll cave, otherwise false"
  (not (equal (get-att name) 'troll)))

(defun has-food (name)
"Return true if this is a food cave, otherwise false"
  (equal (get-att name) 'food))

(defun has-pit (name)
"Return true if this is a pit, otherwise false"
  (equal (get-att name) 'pit))

(defun is-unexplored (name)
"Return true if this is an unexplored cave"
  (equal (third (get-info name)) 'unexplored))

(defun set-info (name value)
"Set the info for a cave, either explored/unexplored or an attribute, or the children when passing a list as the value"
  (when (or (equal value 'explored) (equal value 'unexplored))
    (setf (third (gethash name caves)) value)
    (return-from set-info))
  (when (or (equal value 'start)
            (equal value 'end)
            (equal value 'troll)
            (equal value 'food)
            (equal value 'pit)
            (equal value 'harmless))
    (setf (second (gethash name caves)) value)
    (return-from set-info))
  (setf (first (gethash name caves)) value))

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
      (return-from get-start (list name)))))

(defun explore (name)
"Set a cave to explored and return its unexplored children"
  (set-info name 'explored)

  (setf unexplored nil);a variable to save unexplored children

  (loop for child in (get-children name) do

    (when (and (is-unexplored child) (is-troll-free child))
      ;when a child is unexplored, set it to explored
      ;and save it in the variable unexplored to return later
      (push child unexplored) 
      (setf (gethash child parents) name)
      (set-info child 'explored)))
  unexplored)

(defun breadth-first ()
"Perform a breadth-first search and return the end cave, modifying
  the hashtable parents to record the path"
  (setf q (get-start)) ;q is a queue of caves to search in FIFO
  (loop for name in q do
    (print "Head of q is")(print name)

    ;Return the end cave when it is found
    (when (is-end name) (return-from breadth-first name))

    ;Find all the children of the current cave
    (setf children (explore name))
    (print "Children are")(print children)

    ;add children to the end of the q
    (loop for child in children do
      (setf (cdr (last q)) (list child))))
  nil)

(defun find-path (end)
"Finds the path going up the tree to the start cave from the end cave"
  (when (equal end nil) (return-from find-path "No path found"))
  (setf food 5)
  (setf path (list end))
  (setf cur end)
  (loop
    (when (is-start cur) (return-from find-path (list path food)))
    (when (has-food cur) (incf food 5))
    (when (has-pit cur) (decf food 5))
    (decf food)
    (setf cur (gethash cur parents))
    (push cur path)))

(defun main ()
  ;Initialize cave system - the caves hashtable
  (init)

  ;Run search for the exit, and track moves in the parents hashtable
  (setf end (breadth-first))

  ;Print final exit
  (print "Found end cave named")(print end)

  ;print debug info about all caves
  (print "Caves are formated like this: name ((children) attribute (un)explored)")
  (print "Caves were")(print-all-caves)

  ;Calculate and Print path taken
  (setf path-and-food (find-path end))
  (setf path (first path-and-food))
  (setf food (second path-and-food));is there a way to shorten this?
  (print "Path was")(print path)
  (print "Food at the end was")(print food)

  ;wait for user input at the end rather than auto-quiting
  (read-line))

(main)
