#!/usr/bin/ruby

# Pulseaudio volume control
class Pulse
  attr_reader :volumes, :mutes

  # Constructor
  def initialize
    dump = `pacmd dump`.lines
    @volumes = {}
    @mutes = {}

    # Find the volume settings
    dump.each do |line|
      args = line.split

      # Volume setting
      if args[0] == "set-sink-volume" then
        @volumes[args[1]] = args[2].hex
      end

      # Mute setting
      if args[0] == "set-sink-mute" then
        @mutes[args[1]] = args[2] == "yes"
      end
    end
  end

  # Adjust the volume with the given increment for every sink
  def volume_set_relative(increment)
    @volumes.keys.each do |sink|
      volume = @volumes[sink] + increment
      volume = [[0, volume].max, 0x10000].min
      @volumes[sink] = volume
      `pacmd set-sink-volume #{sink} #{"0x%x" % volume}`
    end
  end

  # Turn the music up!
  def volume_up
    volume_set_relative 0x1000
  end

  # Turn the music up!
  def volume_max
    volume_set_relative 0x20000
    unmute
  end

  # ... and down again
  def volume_down
    volume_set_relative -0x1000
  end

  # Toggle the mute setting for every sink
  def unmute
    @mutes.keys.each do |sink|
      #mute = not(@mutes[sink])
      mute = false
      @mutes[sink] = mute
      `pacmd set-sink-mute #{sink} #{mute ? "yes" : "no"}`
    end
  end
end

# Control code
p = Pulse.new
if ARGV.first == "max" then p.volume_max
elsif ARGV.first == "down" then p.volume_down
elsif ARGV.first == "unmute" then p.unmute else
p.volume_max end
