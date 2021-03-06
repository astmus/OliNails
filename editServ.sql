/****** Script for SelectTopNRows command from SSMS  ******/

alter FUNCTION [dbo].[MaterialUsingStat]
(
    @materialId int
)
RETURNS TABLE AS RETURN
(
	with prepTab as
    (select UsedMaterials.nailDateId, UsedMaterials.materialId, serviceId, name, dbo.GetPriceOfService(serviceId,startTime) as price from UsedMaterials,Services,MaterialServices,NailDates where UsedMaterials.materialId = MaterialServices.materialId and
																	  MaterialServices.serviceId = Services.id and 																	  
																	  UsedMaterials.materialId = 16 and 
																	  NailDates.Id = UsedMaterials.nailDateId)
	select materialId, serviceId, name, sum(price) as totalPrice, count(name) as usedCount from prepTab group by materialId, serviceId, price,name
)
select materialId, serviceId = max(serviceId) , name = ISNULL(name,N'суммарно'), useCount = SUM(useCount), totalPrice = sum(price)
select * from MaterialUsingStat(17) group by rollup(name),materialId

select * from Services
select * from UsedMaterials, Materials, MaterialServices,Services where Materials.id = UsedMaterials.materialId and MaterialServices.materialId = Materials.id and UsedMaterials.materialId = 17 and Services.id = MaterialServices.serviceId

select * from MaterialUsedCount 
alter view MaterialUsedCount as
with srvc as
(select s.name,duration,abbreviation,isObsolete,pos,startTime as materialStartTime,expiredTime,materialId,serviceId
 from Services s, Materials, MaterialServices
 where s.id = MaterialServices.serviceId and Materials.id = MaterialServices.materialId and MaterialServices.materialId = Materials.id), inns as (select nailDateId,StartTime,serviceId from NailDateService inner join NailDates on NailDateService.nailDateId = NailDates.id)
select name,sv.duration as serviceDuration,abbreviation,isObsolete,pos,sv.materialStartTime,expiredTime,materialId,sv.serviceId,count(inns.nailDateId) as useCount,ISNULL(sum(dbo.GetPriceOfService(sv.serviceId,inns.StartTime)),0) as totalPrice
from srvc sv left outer join inns on (inns.serviceId = sv.serviceId and inns.StartTime > sv.materialStartTime and inns.StartTime <= ISNULL(sv.expiredTime,getdate())) 
group by sv.name,sv.duration,abbreviation,isObsolete,pos,sv.materialStartTime,expiredTime,materialId,sv.serviceId

select materialId, serviceId = max(serviceId) , name = ISNULL(name,N'суммарно'), useCount = SUM(useCount), totalPrice = sum(totalPrice) from MaterialUsedCount where ([materialId] = 16) group by rollup(name),materialId
select * from Materials

CREATE FUNCTION AvailableUsedMaterials(@nailDateId int)
RETURNS TABLE
AS
RETURN
select m.id,m.name,m.startTime, CAST(ISNULL(um.materialId, 0) as bit) as selected  from Materials m left join (select * from UsedMaterials where nailDateId = @nailDateId)  um on m.id = um.materialId where id in (select materialId from MaterialServices where serviceId in (select serviceId from NailDateService where nailDateId = @nailDateId))

select * from Services s, Materials, MaterialServices where s.id = MaterialServices.serviceId and Materials.id = MaterialServices.materialId and MaterialServices.materialId = Materials.id and materialId = 3

alter proc StartNewMaterial(@oldMaterialId int)
as
begin
	update Materials set expiredTime = getdate() where id = @oldMaterialId
	insert into Materials(name,startTime,price,amount) select name,getdate(),price,amount from Materials where id = @oldMaterialId
	DECLARE	@id int
	set @id = IDENT_CURRENT('Materials')
	insert into MaterialServices(serviceId,materialID) select serviceId,@id from MaterialServices where materialId = @oldMaterialId
end

select Materials.id as materialId,Services.id as serviceId, Services.name,count(Services.id) as useCount, SUM(dbo.GetPriceOfService(Services.id,NailDates.StartTime)) as totalPrice from NailDateService, Services, NailDates, Materials,MaterialServices where NailDates.Id = NailDateService.nailDateId and NailDateService.serviceId = Services.id and Materials.startTime < NailDates.StartTime and Services.id = MaterialServices.serviceId and Materials.id = MaterialServices.materialId and MaterialServices.materialId = Materials.id group by Services.id,Services.name, Materials.id

select materialId, serviceId = max(serviceId), name = ISNULL(name,N'суммарно'), useCount = SUM(useCount), totalPrice = SUM(totalPrice) from MaterialUsedCount where ([materialId] = 3) group by rollup(name),materialId

select * from MaterialUsedCount where materialId = 3

select * from MaterialUsedCount 
alter view MaterialUsedCount as
with srvc as
(select s.name,duration,abbreviation,isObsolete,pos,startTime as materialStartTime,expiredTime,materialId,serviceId
 from Services s, Materials, MaterialServices
 where s.id = MaterialServices.serviceId and Materials.id = MaterialServices.materialId and MaterialServices.materialId = Materials.id), inns as (select nailDateId,StartTime,serviceId from NailDateService inner join NailDates on NailDateService.nailDateId = NailDates.id)
select name,sv.duration as serviceDuration,abbreviation,isObsolete,pos,sv.materialStartTime,expiredTime,materialId,sv.serviceId,count(inns.nailDateId) as useCount,ISNULL(sum(dbo.GetPriceOfService(sv.serviceId,inns.StartTime)),0) as totalPrice
from srvc sv left outer join inns on (inns.serviceId = sv.serviceId and inns.StartTime > sv.materialStartTime and inns.StartTime <= ISNULL(sv.expiredTime,getdate())) 
group by sv.name,sv.duration,abbreviation,isObsolete,pos,sv.materialStartTime,expiredTime,materialId,sv.serviceId

select * from services
select * from NailDates inner join NailDateService on NailDates.id = NailDateService.nailDateId inner join Services on Services.id = NailDateService.serviceId

select * from AbbretiatedNailDates order by nailDateId
SELECT ' ' + abbreviation FROM AbbretiatedNailDates where AbbretiatedNailDates.nailDateId = 113 FOR XML PATH('')

alter view MaterialsWithRelatedProcedures as
with srv as (select abbreviation,materialId from Services, MaterialServices where Services.id = MaterialServices.serviceId)
select m.id,m.name,m.startTime,m.expiredTime,m.amount,m.price,(SELECT ' ' + abbreviation FROM srv where srv.materialId = m.id FOR XML PATH('')) as procedures from Materials m 

select * from ServicePrice where serviceId = 1 and startDate <= GETDATE()
select * from Services,ServicePrice where ServicePrice.serviceId = Services.id and DATEADD(day,5,GETDATE()) between startDate and endDate or DATEADD(day,5,GETDATE()) >= startDate and endDate is null
SELECT dbo.Services.id, dbo.Services.name, dbo.Services.duration, dbo.Services.abbreviation,
                      dbo.ServicePrice.value as price
FROM         dbo.Services INNER JOIN
                      dbo.ServicePrice ON dbo.Services.id = dbo.ServicePrice.serviceId			
where DATEADD(day,0,GETDATE()) >= startDate and DATEADD(day,0,GETDATE()) < ISNULL(endDate,'01.01.2970')

with priceInfo as (select startDate,value,serviceId from ServicePrice where DATEADD(day,0,GETDATE()) < startDate and endDate is null)
SELECT serv.id, serv.name, serv.duration, serv.abbreviation,
                      dbo.ServicePrice.value as price,
                      (select startDate from priceInfo where priceInfo.serviceId = serv.id)  as sinceDate,
                      (select value from priceInfo where serv.id = priceInfo.serviceId) as newPrice
FROM         dbo.Services serv 					
					  INNER JOIN
                      dbo.ServicePrice ON serv.id = dbo.ServicePrice.serviceId	                       
where DATEADD(day,0,GETDATE()) >= startDate and DATEADD(day,0,GETDATE()) < ISNULL(endDate,'01.01.2970')

create function currentDate() returns DATE
begin
 return DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0)
end

select getdate()

select top 1 id from ServicePrice where serviceId = 4 and endDate > GETDATE() and startDate > GETDATE()
select * from ServicePrice where serviceId = 9 and (endDate > DATEADD(dd, DATEDIFF(dd, 0, getdate()),0) or startDate > DATEADD(dd, DATEDIFF(dd, 0, getdate()),0))

alter proc UpdateServiceProc(@id smallint, @name nvarchar(35), @duration smallint, @abbreviation nvarchar(3), @price smallint, @sinceDate datetime, @result nvarchar(30) output)
as
begin
		declare @curdate datetime = DATEADD(dd, DATEDIFF(dd, 0, getdate()),0)
		update Services set name=@name, duration=@duration, abbreviation=@abbreviation where id=@id
			if (@sinceDate is not null)
			begin
			if ((select count(*) from ServicePrice where serviceId = @id and (endDate > @curdate or startDate > @curdate)) = 2) --then update
				begin
					update ServicePrice set endDate = @sinceDate where serviceId = @id and endDate > @curdate
					update ServicePrice set startDate = @sinceDate, value = @price where serviceId = @id and (startDate > @curdate and endDate is null)
					set @result = 'update is ok'
					return
				end
			else -- add new recors
				begin			
					update ServicePrice set endDate = @sinceDate where serviceId = @id and (endDate > @curdate or endDate is null)
					insert into ServicePrice (startDate, value, serviceId) values (@sinceDate, @price, @id)
					set @result = 'add is ok'
				end
			end
	return
end

create function UpdateService(@id smallint, @newName nvarchar(35), @newDuration smallint, @newAbbreviation nvarchar(3), @newPrice smallint, @sinceDate datetime)
returns nvarchar
as
begin
declare @result nvarchar(30)
EXEC	[dbo].[UpdateServiceProc] @id, @newName, @newDuration, @newAbbreviation, @newPrice,	@sinceDate, @result output

return @result
end




CREATE PROCEDURE GetServicesForEdit @localTime DateTime
AS
WITH priceInfo as (select startDate as st,value,serviceId from ServicePrice where @localTime < startDate and endDate is null)
SELECT	serv.id,
	serv.name,
	serv.duration,
	serv.abbreviation,
	dbo.ServicePrice.value as price,
	priceInfo.st as sinceDate,
	priceInfo.value as newPrice
FROM    
	dbo.Services serv 					
	INNER JOIN
    dbo.ServicePrice ON serv.id = dbo.ServicePrice.serviceId	                       
    left join
    priceInfo on serv.id = priceInfo.serviceId
WHERE @localTime >= startDate and @localTime < ISNULL(endDate,'01.01.2970') and isObsolete = 0 order by serv.id

or endDate is null

create view AbbretiatedNailDates as
	select nailDateId,serviceId,abbreviation from NailDateService, Services, NailDates where NailDates.Id = NailDateService.nailDateId and NailDateService.serviceId = Services.id

create view FullNailDatesInfo as
with data as (select nailDateId,serviceId,dbo.GetPriceOfService(Services.id,NailDates.StartTime) as price,Services.duration,StartTime,ClientPhone,ClientName from NailDateService, Services, NailDates where NailDates.Id = NailDateService.nailDateId and NailDateService.serviceId = Services.id)
select sum(price) as price, sum(duration) as duration,nailDateId,ClientPhone,CAST([ClientName] AS NVARCHAR(MAX)) AS ClientName,StartTime,(SELECT ' ' + abbreviation FROM AbbretiatedNailDates where AbbretiatedNailDates.nailDateId = data.nailDateId FOR XML PATH('')) as procedures
from data group by nailDateId, ClientPhone, CAST([ClientName] AS NVARCHAR(MAX)),StartTime

select * from FullNailDatesInfo

select dbo.GetPriceOfService(29,cast('2016/12/20' as date))

alter FUNCTION GetPriceOfService(@serviceId int, @startDate datetime) RETURNS int
AS
BEGIN
	set @startDate = CONVERT(date, @startDate)	
   DECLARE @id int
   set @id= (SELECT top 1 dbo.ServicePrice.value as price 
	FROM    
	dbo.Services serv 					
	INNER JOIN
    dbo.ServicePrice ON serv.id = dbo.ServicePrice.serviceId where serv.id = @serviceId and @startDate >= startDate and (@startDate < endDate or endDate is null))
   RETURN @id
END 

