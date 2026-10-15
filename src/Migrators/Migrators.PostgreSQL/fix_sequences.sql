
//sudo -i -u postgres
//psql -d TrPostgre -f /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL/fix_sequences.sql

DO $$
DECLARE
    r RECORD;
    seq_name TEXT;
    max_id BIGINT;
BEGIN
    FOR r IN
        SELECT
            n.nspname AS schema_name,
            c.relname AS table_name,
            a.attname AS column_name,
            pg_get_serial_sequence(quote_ident(n.nspname) || '.' || quote_ident(c.relname), a.attname) AS seq
        FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        JOIN pg_attribute a ON a.attrelid = c.oid
        WHERE c.relkind = 'r'
          AND a.attnum > 0
          AND NOT a.attisdropped
          AND pg_get_serial_sequence(quote_ident(n.nspname) || '.' || quote_ident(c.relname), a.attname) IS NOT NULL
    LOOP
        IF r.seq IS NOT NULL THEN
            EXECUTE format('SELECT COALESCE(MAX(%I), 1) FROM %I.%I', r.column_name, r.schema_name, r.table_name)
                INTO max_id;
            EXECUTE format('SELECT setval(%L, %s, true)', r.seq, max_id);
            RAISE NOTICE 'Set %.% -> %', r.table_name, r.column_name, max_id;
        END IF;
    END LOOP;
END $$;