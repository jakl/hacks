#!/usr/bin/perl
use strict;
use warnings;
while(<>){
  my $line = $_;
  if ($line =~ /Hello (.+) (.+)/i){
    print "First: $1\nSecond: $2\n";
  }
}
