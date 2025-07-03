import {Notifications} from "@mantine/notifications";
import {BrowserRouter, Route, Routes} from "react-router";
import AdminAppShell from "src/AdminAppShell/AdminAppShell.jsx";
import AccountList from "src/ChartOfAccount/AccountList.jsx";
import ItemList from "src/Items/ItemList.jsx";
import CustomerList from "src/Customers/CustomerList.jsx";
import SupplierList from "src/Suppliers/SupplierList.jsx";
import {createTheme, MantineProvider} from "@mantine/core";
import EmployeeList from "src/Employees/EmployeeList.jsx";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import PurchaseOrderIndex from "src/PurchaseOrder/PurchaseOrderIndex.jsx";
import ReceivePurchaseOrder from "src/PurchaseOrder/ReceivePurchaseOrder.jsx";
import AccountsPayableIndex from "./AccountsPayable/AccountsPayableIndex";
import DisbursementIndex from "./Disbursements/DisbursementIndex";
import InventoryIndex from "./Inventory/InventoryIndex";
import InvoiceIndex from "./Invoice/InvoiceIndex";
import CollectionIndex from "./Collection/CollectionIndex";
import ReceivableSubledgerIndex from "./Reports/ReceivableSubledgerIndex";
import PayableSubledgerIndex from "./Reports/PayableSubledgerIndex";

const theme = createTheme({
  fontFamily: 'Roboto Condensed, sans-serif'
})

const queryClient = new QueryClient();

export default function App() {
  return (
    <MantineProvider theme={theme} defaultColorScheme="light">
      <Notifications />

      <QueryClientProvider client={queryClient}>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<AdminAppShell />}>
              <Route path="chart-of-account" element={<AccountList />}/>
              <Route path="items" element={<ItemList />} />
              <Route path="customers" element={<CustomerList />} />
              <Route path="suppliers" element={<SupplierList />} />
              <Route path="employees" element={<EmployeeList />} />
              <Route path="purchase-order" element={<PurchaseOrderIndex />} />
              <Route path="receive-purchase-order" element={<ReceivePurchaseOrder />} />
              <Route path="accounts-payable" element={<AccountsPayableIndex />} />
              <Route path="disbursements" element={<DisbursementIndex />} />
              <Route path="inventory" element={<InventoryIndex />} />
              <Route path="invoice" element={<InvoiceIndex />} />
              <Route path="collection" element={<CollectionIndex />} />
              <Route path="report-ar" element={<ReceivableSubledgerIndex />} />
              <Route path="report-ap" element={<PayableSubledgerIndex />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </QueryClientProvider>
    </MantineProvider>
  )
}