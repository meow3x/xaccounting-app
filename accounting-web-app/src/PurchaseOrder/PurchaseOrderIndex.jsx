import CrudToolbar from "src/Shared/CrudToolbar.jsx";
import PurchaseOrderForm, {makePurchaseOrderForm} from "src/PurchaseOrder/PurchaseOrderForm.jsx";
import {useForm} from "@mantine/form";
import {useGetAccounts} from "src/ChartOfAccount/api.js";
import {useGetSuppliers} from "src/Suppliers/api.js";
import {useGetItems} from "src/Items/api.js";

export default function PurchaseOrderIndex() {

  return (
    <>
      <CrudToolbar title="Purchase Order" />

      <PurchaseOrderForm />
    </>
  )
}