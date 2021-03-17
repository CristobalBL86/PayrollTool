USE [PayrollDb]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetPayrollResult]
	@p_stdate datetime,
	@p_enddate datetime
as

DECLARE @v_date Datetime, @v_ammount float, @v_nemp int, @v_empid int;  

select fecha, round(monto, 2) as monto
,(select count(1) from AssistanceLog ass where cast(ass.Date as Date) = tbl.fecha) as Employees
INTO #AmmountPerDay
from (
select cast(trx.Date as Date) as fecha--, trx.ProductId, trx.OperationId, 
	,SUM(p.UnitPrice*trx.BoxQty) monto
from transacionlog trx
	inner join Price p 
		on trx.OperationId = p.OperationId 
		and trx.ProductId = p.ProductId
where (Cast(trx.Date as Date) >= @p_stdate and Cast(trx.Date as Date) <= @p_enddate)
group by cast(trx.Date as Date))tbl

delete from PayrollRelease
where (Cast(date as Date) >= @p_stdate and Cast(Date as Date) <= @p_enddate)

DECLARE pay_cursor CURSOR FOR  
select * from #AmmountPerDay

OPEN pay_cursor
FETCH NEXT FROM pay_cursor  
INTO @v_date, @v_ammount, @v_nemp;  
  
WHILE @@FETCH_STATUS = 0  
BEGIN  
  
	declare employee_Cursor CURSOR FOR
	Select employeeid from AssistanceLog assl where Cast(assl.Date as Date) = @v_date  

	OPEN employee_Cursor
	FETCH NEXT FROM employee_Cursor  
	INTO @v_empid;  

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	   
	   	INSERT INTO PayrollRelease(Date, Employeeid, Ammount)
		VALUES(
			@v_date,
			@v_empid,
			Round(@v_ammount / @v_nemp , 2)	
		)

		FETCH NEXT FROM employee_Cursor  
		INTO @v_empid;  
	END

	CLOSE employee_Cursor;  
	DEALLOCATE employee_Cursor;  
  
	FETCH NEXT FROM pay_cursor  
	INTO @v_date, @v_ammount, @v_nemp;  
END  
  
CLOSE pay_cursor;  
DEALLOCATE pay_cursor;  

DROP TABLE #AmmountPerDay

select * 
from PayrollRelease pr
where (Cast(pr.Date as Date) >= @p_stdate and Cast(pr.Date as Date) <= @p_enddate)