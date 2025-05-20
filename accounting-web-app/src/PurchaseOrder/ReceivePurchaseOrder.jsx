import {
  Button,
  Divider,
  Fieldset,
  Group,
  Notification,
  NumberInput,
  Select,
  Stack,
  Text,
  TextInput
} from "@mantine/core";
import {useGetAccounts} from "src/ChartOfAccount/api.js";
import {useGetSuppliers} from "src/Suppliers/api.js";
import {useGetItems} from "src/Items/api.js";
import {getPurchaseOrder, receivePurchaseOrder, useCreatePurchaseOrder, useGetProjects} from "src/PurchaseOrder/api.js";
import {useState} from "react";
import {useForm} from "@mantine/form";
import {LineItemsList, makePurchaseOrderForm} from "src/PurchaseOrder/PurchaseOrderForm.jsx";
import {DateInput} from "@mantine/dates";
import {useQuery} from "@tanstack/react-query";
import {showSuccessNotification} from "src/util/notification/notifications.js";

function makeReadonlyForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      id: null,
      supplier: null,
      supplierAddress: null,
      requisitionNumber: null,
      deliveryDate: null,
      project: null,
      description: '',
      lineItems: [],
      debitTo: null,
      creditTo: null,
      supplierLimit: 0,

      // Not included for form submission
      meta: {
        vatableAmount: 0,
        vatAmount: 0,
        netAmount: 0
      }
    }
  }
}

export default function ReceivePurchaseOrder() {
  const { data: accounts } = useGetAccounts({
    pagination: {
      page: 1,
      pageSize: 999999 // load all accounts
    },
  });
  const form = useForm(makeReadonlyForm());
  const [items, setItems] = useState([]);
  const [isOpen, setIsOpen] = useState(true);

  function handleSubmit(formData) {
    const request = {
      debitTo: parseInt(formData.debitTo),
      creditTo: parseInt(formData.creditTo)
    }
    const id = form.getValues().id;

    receivePurchaseOrder(id, request).then(() => {
      showSuccessNotification(`You have posted PO#${poNumber} successfully.`)


      form.reset()
      form.setValues({
        id: null,
        supplier: null,
        supplierAddress: null,
        requisitionNumber: null,
        deliveryDate: null,
        project: null,
        description: '',
        lineItems: [],
        debitTo: null,
        creditTo: null,
        supplierLimit: 0,

        // Not included for form submission
        meta: {
          vatableAmount: 0,
          vatAmount: 0,
          netAmount: 0
        }
      })
      setItems([])
    })
  }

  function searchPo() {
    console.log(poNumber)

    getPurchaseOrder(poNumber).then(data => {
      setIsOpen(data.status === 'Open')

      const values = {
        id: data.id,
        supplier: `${data.supplier.supplierId} - ${data.supplier.name}`,
        supplierAddress: addressToString(data.supplier.address),
        requisitionNumber: data.requisitionNumber,
        project: `${data.project.number} - ${data.project.name}`,
        description: data.description,
        deliveryDate: new Date(data.deliveryDate),
        supplierLimit: data.supplier.creditLimit,
        debitTo: data.debitTo.toString(),
        creditTo: data.creditTo.toString(),
        meta: {
          vatableAmount: data.vatableAmount,
          vatAmount: data.vatAmount,
          netAmount: data.netAmount,
        }
      };

      form.setValues(values);
      form.resetDirty(values);

      setItems(data.lineItems.map(e => ({
        id: e.id,
        code: e.itemSnapshot.code,
        quantity: e.quantity,
        uom: e.itemSnapshot.unitOfMeasurement,
        description: e.itemSnapshot.name,
        unitPrice: e.itemSnapshot.unitCost,
        discount: 0,
        lineTotal: e.lineTotal
      })))
    })
  }

  function addressToString(address) {
    if (!address) return ''
    return (address.street || '') + ' '
      + (address.city || '') + ' '
      + (address.province || '')
  }

  const [poNumber, setPoNumber] = useState(null)

  return (
    <>
      <Group mb={20}>
        <Text size="sm">Pay for PO</Text>

        <NumberInput
          value={poNumber}
          onChange={setPoNumber}
          // label="Pay for PO number"
          placeholder="Enter PO number"
        />

        <Button variant="light" onClick={searchPo}>Find PO</Button>
      </Group>

      { isOpen ? <></> :
          <Notification title="Closed PO" color="orange">
            Purchase order status is already closed.
          </Notification>
      }
      <Divider label="Purchase order details"></Divider>

      <form onSubmit={form.onSubmit(handleSubmit)}>
        <Group grow justify="space-between" mb={15}>
          <Stack spacing={2} justify="flex-start" align="stretch">
            <Group grow>

              <TextInput
                required
                readOnly
                label="Supplier"
                key={form.key('supplier')}
                {...form.getInputProps('supplier')}
              />

              <NumberInput
                readOnly
                variant="filled"
                label="Payable balance"
              />

              {/*<Button variant="subtle" size="sm">Open PO</Button>*/}
            </Group>

            <TextInput
              readOnly
              variant="filled"
              label="Address"
              key={form.key('supplierAddress')}
              {...form.getInputProps('supplierAddress')}
            />

            <Group grow>
              <DateInput
                readOnly
                variant="filled"
                defaultValue={new Date()}
                label="Date"
              />

              <NumberInput
                readOnly
                thousandSeparator=","
                variant="filled"
                label="Limit"
                key={form.key('supplierLimit')}
                {...form.getInputProps('supplierLimit')}
              />

            </Group>

            <Group grow>
              <DateInput
                required
                readOnly
                label="Delivery Date"
                placeholder="Select delivery date"
                key={form.key('deliveryDate')}
                {...form.getInputProps('deliveryDate')}
              />

              <TextInput
                required
                readOnly
                label="JO Number"
                key={form.key('project')}
                {...form.getInputProps('project')}
              />
            </Group>

          </Stack>

          <Stack spacing={2} justify="flex-start" align="stretch" gap="md" h={300}>
            <NumberInput
              required
              readOnly
              label="RS Number"
              key={form.key('requisitionNumber')}
              {...form.getInputProps('requisitionNumber')}
            />

            <TextInput
              required
              readOnly
              label="Description"
              key={form.key('description')}
              {...form.getInputProps('description')}
            />
          </Stack>
        </Group>

        <LineItemsList lineItems={items}/>

        <Group grow mt={15}>
          <Group grow justify="flex-start">
            <Fieldset>
              <Select
                required
                readOnly={!isOpen}
                label="Inventory Debit"
                placeholder="Select account to debit"
                searchable
                checkIconPosition="right"
                data={accounts?.records?.map(e => ({
                  value: e.id.toString(),
                  label: `${e.accountId} - ${e.name} (${e.accountType.name})`
                }))}
                key={form.key('debitTo')}
                {...form.getInputProps('debitTo')}
              />

              <Select
                mt="md"
                required
                readOnly={!isOpen}
                label="Payable Credit"
                placeholder="Select account to credit"
                searchable
                checkIconPosition="right"
                data={accounts?.records?.map(e => ({
                  value: e.id.toString(),
                  label: `${e.accountId} - ${e.name} (${e.accountType.name})`
                }))}
                key={form.key('creditTo')}
                {...form.getInputProps('creditTo')}

              />
            </Fieldset>

          </Group>

          <Group grow>
            <Fieldset>
              <NumberInput
                readOnly
                label="VATable Amount"
                variant="filled"
                thousandSeparator=","
                key={form.key('meta.vatableAmount')}
                {...form.getInputProps('meta.vatableAmount')}
              />

              <NumberInput
                readOnly
                label="VAT Amount"
                variant="filled"
                thousandSeparator=","
                key={form.key('meta.vatAmount')}
                {...form.getInputProps('meta.vatAmount')}
              />

              <NumberInput
                readOnly
                label="Discounted"
                variant="filled"
                value={0}
              />

              <NumberInput
                readOnly
                label="NET Amount"
                variant="filled"
                thousandSeparator=","
                key={form.key('meta.netAmount')}
                {...form.getInputProps('meta.netAmount')}/>
            </Fieldset>

          </Group>
        </Group>

        <Group justify="flex-end" mt="md">
          <Button disabled={!isOpen} type="submit">PO Received</Button>
        </Group>
      </form>
    </>

  )
}