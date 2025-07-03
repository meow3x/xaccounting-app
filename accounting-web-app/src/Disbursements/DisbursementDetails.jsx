import { Box, Button, Group, NumberInput, Paper, Stack, TextInput } from "@mantine/core";
import { DateInput } from "@mantine/dates";
import { IconCashBanknote } from "@tabler/icons-react";
import { JournalEntrySimple } from "src/AccountsPayable/AccountsPayableForm";

export default function DisbursementDetails({payment, onPrintCheckRequest}) {
  return (
    <Paper shadow="md" withBorder>
      <Group align="flex-start" m="md">
        <Stack flex={1}>
          <NumberInput
            readOnly
            variant="filled"
            label="CV Number"
            value={payment.voucherNumber}
          />

          <DateInput
            readOnly
            variant="filled"
            label="Date Created"
            value={new Date(payment.createdAt)}
          />

          <TextInput
            readOnly
            variant="filled"
            label="Payee"
            value={`${payment.payee.supplierId} - ${payment.payee.name}`}
            // value={payment?.supplier.name}
          />

          {/* {selectedSupplier ? <SupplierBasicDetails supplier={selectedSupplier} /> : null} */}

          <TextInput
            readOnly
            variant="filled"
            label="Reference Number"
            value={payment.referenceNumber}
          />

          <NumberInput
            readOnly
            variant="filled"
            label="A/P Voucher #"
            value={payment.apvNumber}
          />
        </Stack>

        <Stack flex={3}>
          <Box>
            <JournalEntrySimple lines={payment?.journalEntry.lines ?? []}  />
          </Box>
        </Stack>
      </Group>
      <Group justify="right" m="md">
        <Button
          disabled={payment.chequeStatus !== 'Pending'}
          variant="outline"
          color="dark"
          >Print Check</Button>
      </Group>

    </Paper>
  )
}