#!/usr/bin/python
import re

def quote(x):
  """Return string x with quotes around it"""
  if not re.search(' ',x):
    return x
  elif re.search("'",x):
    return '"'+x+'"'
  else:
    return "'"+x+"'"

source = open('abilities.html').read()

ability_expr=r'http[^\s]+>([-\w. ]+)<.*?<td>(.+?)</td>'
# name description

print 'var abilities = {'

abilities = re.findall(ability_expr, source, re.DOTALL)
for ability in abilities:
  print quote(ability[0])+':{'
  print 'description:'+quote(ability[1])
  print '},'

print '}'
