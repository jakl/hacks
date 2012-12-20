#!/usr/bin/env coffee
#WARNING: Quite often we ignore excess dimensions, assuming equal values, look at minrange
#TODO: merge equal and megaequal to test equality recursively
argf = new (require './argf.js')()
board = []
points = []
argf.forEach (line)->
  if matches = line.match /(\d+)/
    board.addpoint [matches[1].toNums()]
    console.log board #Have board track points and lines seperately. Make more efficient, calculate some line function

Array::addpoint = (point)-> #n'd point - board now contains point and knows if it is close to a solution
  for i in @
    if (a = i.concat point).isline()
      @push a unless @containspoints a
      for j in @
        if (b = j.concat a).isline()
          @push b unless @containspoints b
  @push point unless @containspoint point

Array::containspoints = (points)->
  for i in @
    return true if [points,i].megaequal()
  false
Array::containspoint = (point)->
  for i in @
    return true if [point,i].equal()
  false
String::toNums = -> parseInt num for num in @ #string of digits - array of digits
Array::min = -> Math.min.apply null, @
Array::minrange = -> [0...@map((a)->a.length).min()] #list of lists - range from zero ... to the minimum among lengths
Array::subtract = (a)-> @[i] - a[i] for i in [@,a].minrange()
Array::mag = -> Math.sqrt @map((i)->i*i).reduce (a,b)->a+b #Magnitude of an n'd vector
Array::equal = -> #list of nd points - if each point's dimensions are equal
  for i in [1...@length]
    for j in @minrange()
      return unless @[i-1][j] is @[i][j]
  true
Array::megaequal = -> #list of list of nd points - if each point list's dimensions are equal throughout points
  for i in [1...@length]
    for j in @minrange()
      a = @[i-1][j].sortbymag()
      b = @[i][j].sortbymag()
      for i in [a,b].minrange()
        return unless [a[i], b[i]].equal()
  true
Array::isline = ->
  @sortbymag()
  vector = @[0].subtract @[1]
  for i in [2...@length]
    return unless [@[i-1].subtract(@[i]), vector].equal()
  true
Array::sortbymag = ->
  max = 0
  maxpoint
  magmap =
    for i in [1...@length]
      for j in [i...@length]
        if max < mag = @[i-1].subtract(@[j]).mag()
          max = mag
          maxpoint = i-1
        mag
  #Push distances from maxpoint to the front of each point for sorting
  for i in [0...@length]
    if i is maxpoint
      @[i].unshift 0
    else if maxpoint < i
      @[i].unshift magmap[maxpoint][i-maxpoint-1]
    else if i < maxpoint
      @[i].unshift magmap[i][maxpoint-i-1]
  @sort (a,b)->b[0]-a[0]
  i.shift() for i in @ #Remove sorting distances
  @
