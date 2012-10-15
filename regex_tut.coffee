#!/usr/bin/env coffee
argf = new (require './argf.js')()
argf.forEach (line)->
  if matches = line.match /Hello (.+) (.+)/i
    console.log "First: #{matches[1]}\nSecond: #{matches[2]}"
