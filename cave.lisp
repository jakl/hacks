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
  ;Uncomment this block for a slightly more interested cave system
  ;(cave 0 :children '(3 2 1) :att 'start)
  ;(cave 1 :children '(4 3 2))
  ;(cave 2 :children '(4 3 2))
  ;(cave 3 :children '(4 5))
  ;(cave 4 :children '(42))
  ;(cave 5 :children '(42 33))
  ;(cave 33 :children '(5))
  ;(cave 42 :att 'end)

  ;Comment this block to rid the program of a boring cave system
  (cave 'a :children '(b c) :att 'start)
  (cave 'b :children '(z))
  (cave 'c :children '(z))
  (cave 'z :att 'end))

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
"Return true if this is an end cave, otherwise false"
  (equal (get-att name) 'start))

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
"Set a cave to explored and return its unexplored children
Also delete children from it that were previously explored"
  (set-info name 'explored)

  (setf unexplored nil);a variable to save unexplored children

  (loop for child in (get-children name) do

    (when (is-unexplored child) 
      ;when a child is unexplored, set it to explored
      ;and save it in the variable unexplored to return later
      (push child unexplored) 
      (setf (gethash child parents) name)
      (set-info child 'explored)))
  unexplored)

(defun breadth-first ()
"Perform a breadth-first search and return the path taken to find an end cave
Currently this function returns the end cave, rather than the path taken"
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
  (setf path (list end))
  (setf cur end)
  (loop
    (when (is-start cur) (return-from find-path path))
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

  ;Print path taken
  (print "Path was")(print (find-path end))

  ;wait for user input at the end rather than auto-quiting
  (read-line))

(main)
