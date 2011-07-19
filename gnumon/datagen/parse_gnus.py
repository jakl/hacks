#!/usr/bin/python
import re

def quote(x):
  """Return string x with quotes around it"""
  if re.search("'",x):
    return '"'+x+'"'
  else:
    return "'"+x+"'"

def get_evolution_data():
  """ Parse the file evolves.html
  Turn the data into a dictionary variable and return it
  """

  evolutions = {}

  f = open('evolves.html', 'r')
  source = f.read()

  evolveExpr=r'>([ \w.-]+)<.*?>([ \w.-]+)<.*?>(\d\d?)<'

  evolve = re.findall(evolveExpr, source, re.DOTALL)
  for i in range(len(evolve)-1):
    evolutions[evolve[i][0]]={'name':evolve[i][1],'level':evolve[i][2]}
  f.close()

  return evolutions

evolutions = get_evolution_data()

nameExpr=r'<title>(.+) pokedex'
tp=r'<a  href="http://pokemondb.net/type/' #type prefix
typeExpr=tp+r'.*?>(\w+)</a>\s+<.*?>([\w-]+)<'
catExpr=r'>([-.\w ]+) Pok'
heightExpr=r'([\d.]+)m'
weightExpr=r'([\d.]+) kg'
abilityExpr=r'ability/([-.\w ]+)'
maleExpr=r'male">([\d.]+)'
femaleExpr=r'female">([\d.]+)'
catchExpr=r'(\d+) <small>\('
japExpr=r'Japanese</th>.*?<td>([-.\w ]+)<'
statsExpr=r'c">(\d\d).*c">(\d\d).*c">(\d\d).*c">(\d\d).*c">(\d\d).*c">(\d\d)'
#         HP  Attack  Defense  Sp.Atk  Sp.Def  Speed
descBlockExpr=r'Pokedex descriptions</h2>(.*?)</table>'
descExpr=r'<td>(.*?)</td>'
movesBlockExpr=r'data pokedex-moves-level(.*?)</table>'
movesExpr=r'(\d\d?).*?>([\w -]+)<.*?type (\w+).*?(\w+)</span.*?>([-\d]+)<.*?>([-\d]+)<'
#              lvl     name           type  category         power      accuracy

print 'var gnudex = {'

for i in range(1,647):
  f = open('gnus/'+str(i),'r')
  source = f.read()

  try:
    name = re.search(nameExpr, source).group(1)
    print quote(name)+':{'

    x = re.search(typeExpr, source)
    if x.group(2) != '-':
      print 'type:['+quote(x.group(1))+','+quote(x.group(2))+'],'
    else:
      print 'type:['+quote(x.group(1))+'],'

    if name in evolutions:
      print 'evolution:'+quote(evolutions[name]['name'])+','
      print 'evolve_level:'+evolutions[name]['level']+','

    print 'category:' + quote(re.search(catExpr, source).group(1))+','
    print 'height:' + re.search(heightExpr, source).group(1)+','
    print 'weight:'+ re.search(weightExpr, source).group(1)+','
    print 'ability:' + quote(re.search(abilityExpr, source).group(1))+','
    try:
      print 'male:' + re.search(maleExpr, source).group(1)+','
      print 'female:' + re.search(femaleExpr, source).group(1)+','
    except:
      pass #genderless

    print 'catch_rate:' + re.search(catchExpr, source).group(1)+','

    try:
      print 'japanese_name:' + quote(re.search(japExpr, source, re.DOTALL).group(1))+','
    except:
      pass #US name is already japanese name, untranslated

    x = re.search(statsExpr, source,re.DOTALL)
    print 'hp:'+x.group(1)+','
    print 'attack:'+x.group(2)+','
    print 'defense:'+x.group(3)+','
    print 'special_attack:'+x.group(4)+','
    print 'special_defense:'+x.group(5)+','
    print 'speed:'+x.group(6)+','
    
    print 'descriptions:['
    descBlock = re.search(descBlockExpr, source, re.DOTALL).group(1)
    descs = re.findall(descExpr, descBlock, re.DOTALL)
    for j in descs:
      print ''+quote(j)+','
    print '],'
    
    print 'moves:{'
    movesBlock = re.search(movesBlockExpr, source, re.DOTALL).group(1)
    moves = re.findall(movesExpr, movesBlock, re.DOTALL)
    for move in moves:
      print ''+quote(move[1])+':'+move[0]+','
    print '}'
    print '},'#         lvl   name  type  category  power accuracy
      
  except:
    print 'error on number: '+str(i)

print '}'
