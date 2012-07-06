#!/usr/bin/perl
use DBI;
my $dbh = DBI->connect('dbi:Pg:dbname=_FAKE_DB_NAME_','_FAKE_USER_NAME','_FAKE_PASS_WORD_');

my @tables = @{$dbh->selectcol_arrayref("select tablename from pg_tables where schemaname='_FAKE_SCHEMA_NAME_'")};
my @fks;

#loop through tables
#  loop through foreign keys
#    remember for later, and delete
for my $table (@tables) {
    my @cur_fks = @{$dbh->selectall_arrayref(
      "SELECT tc.constraint_name, tc.table_name, kcu.column_name, ccu.table_name AS foreign_table_name, ccu.column_name AS foreign_column_name FROM information_schema.table_constraints AS tc JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name WHERE constraint_type = 'FOREIGN KEY' AND tc.table_name='$table'"
    )};

    $dbh->do('alter table $table drop CONSTRAINT ' . $foreign_key[0]) for my $foreign_key (@cur_fks);

    push @fks, @cur_fks;
}

#loop through tables
#  loop through varchar columns with name='id' or name like '%_id'
#    switch column type to uuid, converting all values
for my $table (@tables) {

    for my $column (@{$dbh->selectcol_arrayref(
        "SELECT column_name FROM information_schema.columns WHERE table_name = '$table' and (column_name = 'id' or column_name like '%_id') and data_type = 'character varying'"
    )}) {
        $dbh->do("alter table $table alter column $column type uuid using $column".'::uuid');
    }
}

#loop through remembered foreign keys
#  recreate foreign key
for my $fk (@fks) {
    my $constraint = $fk[0];
    my $table = $fk[1];
    my $column = $fk[2];
    my $foreign_table = $fk[3];
    my $foreign_column = $fk[4];
    $dbh->do("ALTER TABLE $table ADD CONSTRAINT $constraint FOREIGN KEY ($column) REFERENCES $foreign_table ($foreign_column)");
}
