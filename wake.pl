#!/usr/bin/perl
use warnings;
use strict;

my $alarmTime = shift;
$alarmTime = '' unless $alarmTime;
my $hour = 24;
my $minute = 0;

#Parse user input leniently for a time in hh:mm, or something similar
if($alarmTime =~ /^(\d?\d)[-:.,; ]*(\d\d)?$/){
  $hour = $1 if $1;
  $minute = $2 if $2;
}

die "Bad format of time\n$alarmTime\n" if($hour >= 24 or $hour < 0 or $minute >= 60 or $minute < 0);

#Find the seconds until the alarm should go off
my $alarmSeconds = `date -d $hour:$minute +%s` - `date +%s`;

#If the alarm is set in the past, assume it is ment for the same time tomorrow
my $SECONDS_IN_A_DAY = 86400;
if($alarmSeconds <= 0){
  $alarmSeconds += $SECONDS_IN_A_DAY;
}
printf "Sleeping for %.3f hours, until %d:%02d\n", $alarmSeconds/3600, $hour, $minute;
`sleep $alarmSeconds`;
`crankvolume`;
