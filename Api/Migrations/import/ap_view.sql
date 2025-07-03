
create view view_accounts_payable as 
with CTE_AP_JL as (
	select journalEntryId, sum(jl.debit) debits, sum(jl.credit) credits from journalEntries je 
	inner join journalLines jl on jl.journalEntryId = je.id
	inner join accounts a on a.id = jl.accountid
	where  a.accountTypeId in (11)
	group by journalEntryId
) 
select 
	s.supplierId,
	s.name,
	sum((select debits from CTE_AP_JL where journalEntryId = p.journalEntryId )) debit,
	sum((select credits from CTE_AP_JL where journalEntryId = ap.journalEntryId )) credit,
	(
		sum((select credits from CTE_AP_JL where journalEntryId = ap.journalEntryId ))
		- sum((select debits from CTE_AP_JL where journalEntryId = p.journalEntryId ))
	 	
	 ) payable
from suppliers s
left join  accountsPayable ap on ap.supplierId = s.id
left join payments p on p.apvNumber = ap.voucherNumber
group by s.supplierId, s.name
order by s.name asc
