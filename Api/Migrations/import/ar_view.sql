-- get debits on receivables
create or replace view public.view_accounts_receivable
AS
with CTE_AR_JL as (
	select journalEntryId, sum(jl.debit) debits, sum(jl.credit) credits from journalEntries je 
	inner join journalLines jl on jl.journalEntryId = je.id
	inner join accounts a on a.id = jl.accountid
	where  a.accountTypeId in (3,4)
	group by journalEntryId
)
select
	c.customerid,
	c.name,
	sum((select debits from CTE_AR_JL where journalEntryId = i.journalEntryId )) debit,
	sum((select credits from CTE_AR_JL where journalEntryId = cp.journalEntryId )) credit,
	(sum((select debits from CTE_AR_JL where journalEntryId = i.journalEntryId ))
	 - sum((select credits from CTE_AR_JL where journalEntryId = cp.journalEntryId ))) receivable
from customers c
left join invoices i on i.customerId = c.id
left join collectionPayment cp on cp.invoiceId = i.id
group by c.customerid, c.name
order by c.name asc;


ALTER TABLE public.view_accounts_receivable
    OWNER TO admin;


select * from view_accounts_receivable;
