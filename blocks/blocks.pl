#!/usr/bin/perl
=pod
Copyright 2010 James Koval

This file is part of Blocks

Blocks is free software: you can redistribute it
and/or modify it under the terms of the GNU General Public License
as published by the Free Software Foundation, either version 3
of the License, or (at your option) any later version.

Blocks is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
the GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Blocks. If not, see <http://www.gnu.org/licenses/gpl.html>
=cut
use strict;
use warnings;
use Getopt::Long;
use subs qw(printMatchOrInverse printDataOrBlock);
my $term='.';#Term to search for in a block of text to validate the block
my $match=1; #Either match the term, or inverse match it, getting every block of text except for those containing term
my $delim;   #Delimiter at the heading of each block of text
my $data=0;  #Data member to show from within the block of text, rather than the entire thing
GetOptions('data=s' => \$data, 'match!' => \$match,'term=s' => \$term,'delim=s' => \$delim);
if(!$delim){#require delimiter
print <<EOF;
NAME
  Blocks

USAGE
  $0 [ --term=<str>|--match|--no-match|--data=<str>|--delim=<str> LOCAL_FILE ]

DESCRIPTION
  Use to parse files composed of regular blocks of text

OPTIONS
  --term      -t  : Term to search for
  --match     -m  : Match the term
  --no-match -nom : Match all but the term; inverted match
  --delim     -de : Delimit the blocks of text by this header line (REQUIRED)
                       delimiter header line is included in the block
  --data      -da : Find and print only this bit of data (regex formatted)

  Option names may be shorter unique abbreviations of the full names shown above
  Full or abbreviated options may be preceded by one - or two -- dashes

EXAMPLES
  We are looking at a log file of computers identified by their ip addresses
  It might look like this:

    IP-ADDRESS 10.10.10.10
    OS: xp
    VULNS: Isn't running free software
    IP-ADDRESS 42.42.42.42
    OS: linux
    VULNS: None
    IP-ADDRESS 1.2.3.4
    OS: mac
    VULNS: High risk of social engineering through confident users

  Each computer has its operating system defined, xp, vista, ubuntu
  Each computer has a list of secuirty holes like: VULNS: Default SSH Port, No pass


  We want to list the security holes of computers that aren't running xp

  $0 --term xp --no-match --delim IP-ADDRESS --data 'VULNS:(.*)'
    None
    High risk of social engineering through confident users


  We want to list the blocks of text associated with computers that aren't running xp

  $0 --term xp --no-match --delim IP-ADDRESS trialfile
    IP-ADDRESS 42.42.42.42
    OS: linux
    VULNS: None
    IP-ADDRESS 1.2.3.4
    OS: mac
    VULNS: High risk of social engineering through confident users

    
  We want to list the ips from each block of text

  $0 --delim IP-ADDRESS --data 'IP-ADDRESS (.*)' trialfile
    10.10.10.10
    42.42.42.42
    1.2.3.4


AUTHOR
  Written by James Koval
REPORTING BUGS
  Report bugs to <jediknight304 () gmail . com>
COPYRIGHT
  Copyright 2010 James Koval
  License GPLv3+: GNU GPL version 3 or later
  <http://gnu.org/licenses/gpl.html>
  This is free software; you are free to change and redistribute it
  There is NO WARRANTY, to the extent permitted by law
EOF
exit;
}

my $block;

while(<>){
  if(/$delim/){#find a starting delimiter
    #print and reset the current working block of text
    printMatchOrInverse $match, $block, $term, $data;
    $block = $_;
  }
  else{
    #Continue to slurp the block of text
    $block .= $_;
  }
}

#repeat at end of file without needing a final delimiter
printMatchOrInverse $match, $block, $term, $data;

sub printDataOrBlock{
  my $data = shift;
  my $block = shift;
  if ($data){
    print $1."\n" if $block =~ /$data/i;
  }else{
    print $block ;
  }
}

sub printMatchOrInverse{
  my $match = shift;
  my $block = shift;
  my $term = shift;
  my $data = shift;
  if($block){#if previously saved block of text
    if($match){ #If the user wants to match, rather than inverse match
      if ($block =~ /$term/i){
        printDataOrBlock $data, $block;
      }
    }else{#inverse match
      unless ($block =~ /$term/i){
        printDataOrBlock $data, $block;
      }
    }
  }
}
