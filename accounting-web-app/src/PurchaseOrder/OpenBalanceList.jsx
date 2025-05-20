import {useGetPoBalance} from "src/PurchaseOrder/api.js";

export default function OpenBalanceList() {
  // PO , ordered, received, open
  const { data: balance } = useGetPoBalance();
}