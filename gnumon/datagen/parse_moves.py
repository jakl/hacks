#!/usr/bin/python
import re

def quote(x):
  """Return string x with quotes around it"""
  if re.search("'",x):
    return '"'+x+'"'
  else:
    return "'"+x+"'"

def printData(name, data):
  """Only print if exists, and quote strings properly"""
  if(data != '-' and data != ''):
    try:
      int(data)
      print name+':'+data+','
    except:
      print name+':'+quote(data)+','


f = open('moves.html', 'r')
source = f.read()

moveExpr=r'(.*)\n(.*)\n(.*)\n\n(.*)\n(.*)\n(.*)\n(.*)\n(.*)\n(.*)\n\n\n'
#Name Type Category power accuracy pp TM Description effect-probability

print 'var moves={'

moves = re.findall(moveExpr, source)
for move in moves:
  print quote(move[0])+':{'
  print 'type:'+quote(move[1])+','
  print 'category:'+quote(move[2])+','
  print 'pp:'+move[5]+','
  printData( 'power',move[3])
  printData( 'accuracy',move[4])
  printData( 'tm',move[6])
  printData( 'description',move[7])
  printData( 'probability',move[8])
  print '},'

print '}'
