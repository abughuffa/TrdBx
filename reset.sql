CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260929070912_ToBeInitialCreate') THEN
    ALTER TABLE sms_cursors RENAME TO "SmsCursors";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260929070912_ToBeInitialCreate') THEN
    ALTER TABLE "SmsCursors" RENAME COLUMN value TO "Value";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260929070912_ToBeInitialCreate') THEN
    ALTER TABLE "SmsCursors" RENAME COLUMN key TO "Key";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260929070912_ToBeInitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260929070912_ToBeInitialCreate', '10.0.11');
    END IF;
END $EF$;
COMMIT;

