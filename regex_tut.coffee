#!/usr/bin/env coffee
argv = new (require './argf.js')()
argv.forEach (line)->
  if matches = line.match /Hello (.+) (.+)/i
    console.log "First: #{matches[1]}\nSecond: #{matches[2]}"
