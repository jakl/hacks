#!/usr/bin/perl
# Copyright 2009 James Koval
#
# This file is part of 3dTicTacToe
#
# 3dTicTacToe is free software: you can redistribute it
# and/or modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation, either version 3
# of the License, or (at your option) any later version.
#
# 3dTicTacToe is distributed in the hope that it will be useful, but
# WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
# the GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with 3dTicTacToe. If not, see <http://www.gnu.org/license/>
use strict; use warnings;
use subs qw(display points);
my $player = 1;
my $X = shift; $X = 'X' unless $X;#X player's name
my $O = shift; $O = 'O' unless $O;#O player's name
my $X1 = substr($X,0,1); my $O1 = substr($O,0,1);#First letter in X and O players' names to mark on board
my @board;
while(1){
	display;
	print "\nPlayer: ";
	print $X if $player; print $O unless $player;
	print "\nPoints Before: ".points."  <--Beta Feature! Please test";
	print "\n";
	print "Z? "; my $z = <>; chomp $z; next if $z !~ /[123]/; next if length $z != 1;
	print "X? "; my $x = <>; chomp $x; next if $x !~ /[123]/; next if length $x != 1;
	print "Y? "; my $y = <>; chomp $y; next if $y !~ /[123]/; next if length $y != 1;
	next if defined $board[$z][$x][$y];
	$board[$z][$x][$y]=$player;
	print "Points After: ".points;
	$player= not $player;#flip/flop every loop to track players' turns
}

sub display {
	print "\n";
	for my $z (1..3){
		print "\nZ: $z\n";#show Z tier headers
		print "   1  2  3 X\n";#show X column headers
		for my $y (1..3){
			print "$y  ";#show Y row headers
			for my $x (1..3){
				my $square = $board[$z][$x][$y];#temporary var
				print "~" unless defined $square;#empty space
				if(defined $square){
					print $X1 if $square;#X space
					print $O1 unless $square;#O space
				}
				print "  ";
			}print "\n";
		}print "Y\n";#Label Y axis
	}
}

sub points {
	my $points = 0;
	for my $z (1..3){
		for my $x (1..3){
			for my $y (1..3){
				my $s = $board[$z][$x][$y];#temp var representing current space
				if (defined $s){#if $player == $square
					for my $z1(0,1){
						for my $y1(0,1){
							for my $x1(0,1){
								my $s1 = $board[$z+$z1][$x+$x1][$y+$y1]; next unless defined $s1;
								my $s2 = $board[$z-$z1][$x-$x1][$y-$y1]; next unless defined $s2;
								unless ($z1 == 0 and $y1 == 0 and $x1 == 0){
									 $points++ if $s1==$player and $s2==$player and $s==$player;
								}
							}
						}
					}
				}
			}
		}
	}
	return $points;
}
