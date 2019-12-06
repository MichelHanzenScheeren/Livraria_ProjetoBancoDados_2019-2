/* Ver todos as triggers: */
	SELECT * FROM SYS.TRIGGERS;

/* Ver todas as procedures: */
	SELECT * FROM SYS.PROCEDURES;

/* Ver todas as functions: */
	SELECT * FROM SYS.OBJECTS
	WHERE type_desc LIKE '%FUNCTION%';