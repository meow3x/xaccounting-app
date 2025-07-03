-- View: public.view_endinginventorycost

-- DROP VIEW public.view_endinginventorycost;

CREATE OR REPLACE VIEW public.view_endinginventorycost
 AS
 WITH cte_sales(itemid, purchasecount, salescount) AS (
         SELECT iv.itemid,
            sum(
                CASE
                    WHEN il.quantity >= 0 THEN il.quantity
                    ELSE 0
                END) AS purchasecount,
            abs(sum(
                CASE
                    WHEN il.quantity < 0 THEN il.quantity
                    ELSE 0
                END)) AS salescount
           FROM inventory iv
             JOIN inventorylogs il ON il.inventoryid = iv.id
          GROUP BY iv.itemid
        )
 SELECT i.code AS itemcode,
    i.name AS itemname,
    s.purchasecount::numeric * i.unitcost AS purchase,
    s.salescount::numeric * i.unitprice AS sold,
    s.purchasecount::numeric * i.unitcost - s.salescount::numeric * i.unitprice AS endcost
   FROM items i
     LEFT JOIN cte_sales s ON s.itemid = i.id;

ALTER TABLE public.view_endinginventorycost
    OWNER TO admin;

