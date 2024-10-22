DECLARE @MyTableParam MyTableType;
INSERT INTO @MyTableParam (GuidId)
SELECT GuidId FROM @MyTableParam;

// Here we add our join query
SELECT * 
FROM @MyTableParam TP
INNER JOIN ViewTable VT ON TP.GuidId = VT.GuidId

