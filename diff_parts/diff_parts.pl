#!/usr/bin/perl
use Getopt::Long;
use Diff;
use strict;
use warnings;

my ($a, $b, $header, $cur_header);
GetOptions(
    'a=s' => \$a,
    'b=s' =>\$b,
    'header=s' => \$header,
);

my %header_contents_a; #contains arrays of strings
my $fha;
open $fha, $a;
$cur_header = '';
for (<$fha>){
    chomp;
    $cur_header = $_ if /$header/;
    push @{$header_contents_a{$cur_header}}, $_;
}

my %header_contents_b; #contains arrays of strings
my $fhb;
open $fhb, $b;
$cur_header = '';
for (<$fhb>){
    chomp;
    $cur_header = $_ if /$header/;
    push @{$header_contents_b{$cur_header}}, $_;
}

for my $key (keys %header_contents_a){
    print "$key\n";
    my $diff = Array::Diff->diff(
        \@{$header_contents_a{$key}},
        \@{$header_contents_b{$key}}
    );
    print ">\n" . join("\n", @{$diff->added}) . "\n" if @{$diff->added};
    print "<\n" . join("\n", @{$diff->deleted}) . "\n" if @{$diff->deleted};
}
