/* journal entries */
select jt.name journalType,
	je.id journalEntryLink,
	ac.accountId,
	at.name accountTypeName,
	ac.name accountName,
	jl.referenceNumber1 reference,
	jl.description,
	coalesce(jl.debit::text, '-') debit,
	coalesce(jl.credit::text, '-') credit,
	jl.createdat timestamp
from journalEntries je
inner join journalTypes jt on jt.id = je.journalTypeId
inner join journallines jl on jl.journalentryid = je.id
inner join accounts ac on ac.id = jl.accountId
inner join accountTypes at on at.id = ac.accountTypeId

/* open purchase order balance */
select po.number,
	po.netamount ordered,
	case when status = 0
		then po.netamount
		else 0
	end as received,
	(
		po.netamount - (case when status = 0
							then po.netamount
							else 0
						end)
	) openamt
from purchaseOrders po

