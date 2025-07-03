import { Box, Button, Fieldset, Group, NumberInput, Paper, Select, Stack, Text, TextInput } from "@mantine/core";
import { DateInput } from "@mantine/dates";
import { useForm } from "@mantine/form";
import { useGetAccounts } from "src/ChartOfAccount/api";
import { useGetCustomers } from "src/Customers/api";
import { useGetItems } from "src/Items/api";
import { ItemSelectorApplet } from "src/Items/ItemSelectorApplet";
import SalesSummary from "./SalesSummary";
import { useState } from "react";
import { useCreateInvoice } from "./api";
import { useQueryClient } from "@tanstack/react-query";
import { showError, showSuccessNotification } from "src/util/notification/notifications";

const PAYMENT_METHODS = [
  {
    id: 1,
    label: "Cash"
  },
  {
    id: 2,
    label: "On Account"
  }
]

function makeInvoiceFormData() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      paymentMethod: '1',
      customerId: null,
      referenceNumber: null,
      debitTo: null,
      creditTo: null,
    }
  }
}

export default function InvoiceForm({onClose}) {
  const form = useForm(makeInvoiceFormData())
  const { data: customers } = useGetCustomers()
  const { data: items } = useGetItems()
  const { data: accounts } = useGetAccounts({
    pagination: {
      page: 1,
      pageSize: 999999 // load all accounts
    }
  })
  const [lineItems, setLineItems] = useState([]) // Merged upon submission
  const mutation = useCreateInvoice()
  const queryClient = useQueryClient()

  const netAmount = lineItems.map(e => e.lineTotal).reduce((acc, curr) => {
    return acc + curr;
  }, 0)
  const vatable = Math.round((netAmount / 1.12) * 100) / 100
  const vatAmount = Math.round(vatable * 0.12 * 100) / 100

  function handleItemAdded(item) {
    setLineItems(prev => [ ...prev, {
      itemId: item.id,
      quantity: item.quantity,
      discount: 0,
      lineTotal: item.lineTotal
    }])
  }

  function handleSubmit(formData) {
    if (!lineItems.length) {
      console.log('Empty line item')
      showError('Please input some line items')
      return
    }

    const request = {
      paymentMethod: parseInt(formData.paymentMethod),
      customerId: parseInt(formData.customerId),
      referenceNumber: formData.referenceNumber,
      creditTo: parseInt(formData.creditTo),
      debitTo: parseInt(formData.debitTo),
      lineItems: lineItems.map(e => ({
        itemId: parseInt(e.itemId),
        quantity: e.quantity,
        discount: e.discount
      })),
    }

    console.log('request', request)

    mutation.mutate(request, {
      onSuccess: (response) => {
        console.log('response', response)
        showSuccessNotification(`Invoice created with number ${response.number}`)
        form.reset()
        setLineItems([])

        queryClient.invalidateQueries({queryKey: ['invoices']})
        queryClient.invalidateQueries({queryKey: ['invoice-on-account']})
        onClose?.()
      }
    })
  }

  return (
    <form onSubmit={form.onSubmit(handleSubmit)}>
      <Text>Details</Text>

      <Paper withBorder w="100%">
        <Group m="md" grow>
          <Select
            required
            label="Payment Method"
            placeholder="Select payment method"
            checkIconPosition="right"
            data={PAYMENT_METHODS.map(e => ({
              value: e.id.toString(),
              label: e.label
            }))}
            key={form.key('paymentMethod')}
            {...form.getInputProps('paymentMethod')}
          />

          <Select
            required
            searchable
            label="Customer"
            placeholder="Select customer"
            checkIconPosition="right"
            data={customers?.map(e => ({
              value: e.id.toString(),
              label: `${e.customerId} - ${e.name}`
            }))}
            key={form.key('customerId')}
            {...form.getInputProps('customerId')}
          />

          <DateInput
            readOnly
            variant="filled"
            defaultValue={new Date()}
            label="Date"
          />

          <TextInput
            label="Invoice Reference"
            key={form.key('referenceNumber')}
            {...form.getInputProps('referenceNumber')}
          />
        </Group>
      </Paper>

      <Text mt="lg">Line Items</Text>

      <Paper withBorder w="100%">
        <ItemSelectorApplet itemList={items} onItemAdded={handleItemAdded} isSelling={true}/>
      </Paper>

      <Group align="flex-start" mt="lg">
        <Stack flex={1}>
          <Text>Journal Entry</Text>
          <Paper>
            <AccountSelector
              accounts={accounts?.records}
              debitKey={form.key('debitTo')}
              debitProps={form.getInputProps('debitTo')}
              debitLabel="Cash / Receivable"
              creditKey={form.key('creditTo')}
              creditProps={form.getInputProps('creditTo')}
              creditLabel="Sales" />
          </Paper>

        </Stack>
        <Stack flex={1}>
          <Text>Summary</Text>
          <Paper>
            <SalesSummary sale={{
              vatableAmount: vatable,
              vatAmount: vatAmount,
              netAmount: netAmount
            }} />
          </Paper>

          <Button type="submit" >Submit</Button>
        </Stack>
      </Group>
    </form>
  )
}

export function AccountSelector({accounts,
  debitKey, debitProps, debitLabel,
  creditKey, creditProps, creditLabel}) {
  return (
    <Fieldset>
      <Select
        required
        label={debitLabel}
        placeholder="Select account to debit"
        searchable
        checkIconPosition="right"
        data={accounts?.map(e => ({
          value: e.id.toString(),
          label: `${e.accountId} - ${e.name} (${e.accountType.name})`
        }))}
        key={debitKey}
        {...debitProps}
      />

      <Select
        mt="sm"
        required
        label={creditLabel}
        placeholder="Select account to credit"
        searchable
        checkIconPosition="right"
        data={accounts?.map(e => ({
          value: e.id.toString(),
          label: `${e.accountId} - ${e.name} (${e.accountType.name})`
        }))}
        key={creditKey}
        {...creditProps}
      />
    </Fieldset>
  )
}