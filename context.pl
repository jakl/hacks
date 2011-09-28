#!/usr/bin/perl
use Getopt::Long;
use strict;
use warnings;

my ($line_start, $line_end, $line_middle, $regex_start, $regex_end, $width) = (0,0,0,0,0,5);
GetOptions(
    'line_start=i' => \$line_start,
    'line_end=i' =>\$line_end,
    'line_middle=i' =>\$line_middle,
    'regex_start=s' =>\$regex_start,
    'regex_end=s' =>\$regex_end,
    'width=i' =>\$width,
);

if($line_middle){
    $line_start = $line_middle - $width;
    $line_end = $line_middle + $width;
}
if($regex_start){
    my $big_number = 2**42;
    $line_start = $big_number;
    $line_end = $big_number;
}

my $line = 0;
while(<>){
    $line++;
    if($regex_start){
        if(/$regex_start/){
            $line_start = $line;
            $line_end = $line + $width unless $regex_end;
        }
    }
    print "$line:\t$_" if $line >= $line_start and $line <= $line_end;
    last if $line > $line_end or /$regex_end/;
}
