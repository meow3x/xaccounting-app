import {
  ActionIcon,
  Button,
  Divider,
  Fieldset,
  Group,
  NumberInput,
  Paper,
  Select,
  Stack,
  Table, Text,
  Textarea,
  TextInput, Title
} from "@mantine/core";
import {DateInput} from "@mantine/dates";
import {useState} from "react";
import {useForm} from "@mantine/form";
import {useGetAccounts} from "src/ChartOfAccount/api.js";
import {useGetSuppliers} from "src/Suppliers/api.js";
import {useGetItems} from "src/Items/api.js";
import {useCreatePurchaseOrder, useGetProjects} from "src/PurchaseOrder/api.js";
import {IconEqual, IconMinus, IconTrash, IconX} from "@tabler/icons-react";
import {Heading, Peso} from "src/util/table/common.jsx";
import {showSuccessNotification} from "src/util/notification/notifications.js";
import { ItemSelectorApplet } from "../Items/ItemSelectorApplet";

export function makePurchaseOrderForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      supplierId: null,
      requisitionNumber: null,
      deliveryDate: null,
      projectId: null,
      description: '',
      lineItems: [],
      debitTo: null,
      creditTo: null,

      // Not included for form submission
      meta: {
        vatableAmount: 0,
        vatAmount: 0,
        netAmount: 0
      }
    }
  }
}

export default function PurchaseOrderForm() {
  const { data: accounts } = useGetAccounts({
    pagination: {
      page: 1,
      pageSize: 999999 // load all accounts
    },
  });
  const { data: suppliers } = useGetSuppliers();
  const { data: items } = useGetItems();
  const { data: projects } = useGetProjects();
  const [selectedSupplier, setSelectedSupplier] = useState(null);
  const form = useForm(makePurchaseOrderForm());
  const mutation = useCreatePurchaseOrder()

  form.watch('supplierId', ({previousValue, value}) => {
    if (value !== previousValue) {
      const supplier = suppliers?.find(e => e.id === parseInt(value))
      setSelectedSupplier(supplier)
    }
  })

  function handleItemAdded(item) {
    const merged = [
      ...form.getValues().lineItems,
      ...[{
        itemId: item.id,
        quantity: item.quantity,
        discount: 0,
        lineTotal: item.lineTotal
      }]
    ]
    // console.log(merged)
    form.setFieldValue('lineItems', merged)

    // Update amount details
    const netAmount = merged.map(e => e.lineTotal).reduce((acc, curr) => {
      return acc + curr;
    }, 0)
    const vatable = Math.round((netAmount / 1.12) * 100) / 100
    const vatAmount = Math.round(vatable * 0.12 * 100) / 100

    form.setFieldValue('meta.vatableAmount', vatable)
    form.setFieldValue('meta.vatAmount', vatAmount)
    form.setFieldValue('meta.netAmount', netAmount)
  }

  function handleSubmit(formData) {
    // Transform data
    console.log(formData);
    const request = {
      supplierId: parseInt(formData.supplierId),
      requisitionNumber: formData.requisitionNumber.toString(),
      deliveryDate: formData.deliveryDate.toISOString().split("T")[0],
      projectId: parseInt(formData.projectId),
      description: formData.description,
      lineItems: formData.lineItems.map(e => ({
        itemId: e.itemId,
        quantity: e.quantity,
        discount: e.discount,
      })),
      debitTo: parseInt(formData.debitTo),
      creditTo: parseInt(formData.creditTo)
    };

    // console.log('request', request)

    mutation.mutate(request, {
      onSuccess: (response) => {
        console.log('response', response)
        form.reset()
        showSuccessNotification(`Purchase order submitted with PO number: ${response.number}.`)
      }
    })
  }

  function addressToString(address) {
    if (!address) return ''
    return (address.street || '') + ' '
      + (address.city || '') + ' '
      + (address.province || '')
  }

  return (
    <form onSubmit={form.onSubmit(handleSubmit)}>

      {/*<Heading text="Details" />*/}
      {/*<Divider my="md" c="lime.9"  />*/}

      <Group grow justify="space-between" mb={15}>
        <Stack spacing={2} justify="flex-start" align="stretch">
          <Group grow>
            <Select
              required
              label="Supplier"
              placeholder="Select supplier"
              searchable
              checkIconPosition="right"
              data={suppliers?.map(e => ({
                value: e.id.toString(),
                label: `${e.supplierId} - ${e.name}`
              }))}
              key={form.key('supplierId')}
              {...form.getInputProps('supplierId')}
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
            value={addressToString(selectedSupplier?.address)}
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
              value={selectedSupplier?.creditLimit}
            />

          </Group>

          <Group grow>
            <DateInput
              required
              label="Delivery Date"
              placeholder="Select delivery date"
              key={form.key('deliveryDate')}
              {...form.getInputProps('deliveryDate')}
            />

            <Select
              required
              label="JO Number"
              placeholder="Job Order"
              searchable
              checkIconPosition="right"
              data={projects?.map(e => ({
                value: e.id.toString(),
                label: `${e.number} - ${e.name}`
              }))}
              key={form.key('projectId')}
              {...form.getInputProps('projectId')}
            />
          </Group>

        </Stack>

        <Stack spacing={2} justify="flex-start" align="stretch" gap="md" h={300}>
          <NumberInput
            required
            label="RS Number"
            key={form.key('requisitionNumber')}
            {...form.getInputProps('requisitionNumber')}
          />

          <TextInput
            required
            label="Description"
            key={form.key('description')}
            {...form.getInputProps('description')}
          />
        </Stack>
      </Group>

      {/*<Heading text="Items" />*/}
      <Divider my="md" c="lime.9" label="Select items" variant="dotted" />

      <ItemSelectorApplet itemList={items} onItemAdded={handleItemAdded} />

      {/*<Heading text="Summary" />*/}
      {/*<Divider my="md" c="lime.9" />*/}

      <Group grow mt={15}>
        <Group grow justify="flex-start">
          <Fieldset>
            <Select
              required
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
        <Button type="submit">Submit PO</Button>
      </Group>
    </form>
  )
}

export function LineItemsList({lineItems, isSelling}) {
  const body = lineItems.map(item =>
    <Table.Tr key={item.id}>
      <Table.Td>{item.code} - {item.description}</Table.Td>
      <Table.Td>{item.quantity} {item.uom}</Table.Td>
      <Table.Td>{Peso(item.price)}</Table.Td>
      <Table.Td>{Peso(item.discount)}</Table.Td>
      <Table.Td>{Peso(item.lineTotal)}</Table.Td>
      <Table.Td>
        <ActionIcon size="sm" variant="subtle" color="red" >
          <IconX size={16} />
        </ActionIcon>
      </Table.Td>
    </Table.Tr>
  )
  return (
    <Table mt="md" highlightOnHover withColumnBorders>
      <Table.Thead>
        <Table.Tr>
          <Table.Th>Item </Table.Th>
          <Table.Th>Quantity</Table.Th>
          <Table.Th>{isSelling ? 'Unit Price' : 'Unit Cost'}</Table.Th>
          <Table.Th>Discount</Table.Th>
          <Table.Th>Total Price</Table.Th>
          <Table.Th>Controls</Table.Th>
        </Table.Tr>
      </Table.Thead>
      <Table.Tbody>
        {body}
      </Table.Tbody>
    </Table>
  )
}