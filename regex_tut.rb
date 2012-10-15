#!/usr/bin/env ruby

ARGF.each_line do |line|
  matches = line.match(/Hello (.+) (.+)/i);
  if matches
    puts "First: #{matches[1]}\nSecond: #{matches[2]}\n"
  end
end
