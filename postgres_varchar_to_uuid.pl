#!/usr/bin/perl
#Given a schema, change all varchar ID columns to the more efficient UUID type
#Operates on varchar columns where the column name matches id or %_id
#Adds a trigger to autogenerate UUIDs whenever an insert leaves the id as null

#Make sure to enable UUID support for the specific schema in the particular db in postgres
#sudo su postgres -c 'psql -d _FAKE_DB_NAME_'
#CREATE EXTENSION "uuid-ossp" schema _FAKE_SCHEMA_NAME_;

use DBI;
execute(@ARGV);

sub execute {
    my $example = "perl $0 db_name schema_name username password\n";

    my $db_name = shift || die $example;
    my $schema_name = shift || die $example;
    my $username = shift || die $example;
    my $password = shift || die $example;

    my $dbh = DBI->connect("dbi:Pg:dbname=$db_name",$username,$password);
    my @tables = @{$dbh->selectcol_arrayref("select tablename from pg_tables where schemaname='$schema_name'")};

    convert_varchar_id_columns_to_uuid($dbh, @tables);
    add_trigger_to_autogenerate_uuids_on_insert($dbh, @tables);
}

sub convert_varchar_id_columns_to_uuid {
    my $dbh = shift;
    my @tables = @_;
    my @fks;

#loop through tables
#  loop through foreign keys
#    remember for later, and delete
    for my $table (@tables) {
        my @cur_fks = @{$dbh->selectall_arrayref(
          "SELECT tc.constraint_name, tc.table_name, kcu.column_name, ccu.table_name AS foreign_table_name, ccu.column_name AS foreign_column_name FROM information_schema.table_constraints AS tc JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name WHERE constraint_type = 'FOREIGN KEY' AND tc.table_name='$table'"
        )};

        $dbh->do("alter table $table drop CONSTRAINT " . $_->[0]) for (@cur_fks);

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
        my $constraint = $fk->[0];
        my $table = $fk->[1];
        my $column = $fk->[2];
        my $foreign_table = $fk->[3];
        my $foreign_column = $fk->[4];
        $dbh->do("ALTER TABLE $table ADD CONSTRAINT $constraint FOREIGN KEY ($column) REFERENCES $foreign_table ($foreign_column)");
    }
}

sub add_trigger_to_autogenerate_uuids_on_insert {
    my $dbh = shift;
    my @tables = @_;

#Create a function that will autogenerate a UUID to replace a null id
    my $autogenerate_uuid_function = <<'EOF';
    CREATE FUNCTION gen_uuid() RETURNS trigger AS $gen_uuid$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := uuid_generate_v1();
            END IF;
            RETURN NEW;
        END;
    $gen_uuid$ LANGUAGE plpgsql;
EOF
    $dbh->do($autogenerate_uuid_function);

#loop through tables
#  loop through uuid columns with name='id'
#    add a trigger to autogenerate the uuid if it was given as null in an insert
    for my $table (@tables) {
        for my $column (@{$dbh->selectcol_arrayref(
            "SELECT column_name FROM information_schema.columns WHERE table_name = '$table' and column_name = 'id' and data_type = 'uuid'"
        )}) {
            $dbh->do("CREATE TRIGGER gen_uuid BEFORE INSERT ON $table FOR EACH ROW EXECUTE PROCEDURE gen_uuid()");
        }
    }
}
