import {Button, Group, NumberInput, Select, TextInput} from "@mantine/core";

export function makeEmployeeForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      employeeId: '',
      firstName: '',
      lastName: '',
      middleName: null,
      address: {
        street: '',
        city: '',
        province: '',
        landlineNumber: '',
        mobileNumber: ''
      },
      tin: null,
      pagIbigId: null,
      philhealthId: null,
      rate: null,
      salaryUnit: "Hourly"
    }
  }
}

export function EmployeeForm({ formController, handleSubmit, salaryUnits}) {
  return (
    <form onSubmit={formController.onSubmit(handleSubmit)}>
      <TextInput
        required
        label="Employee Id"
        key={formController.key('employeeId')}
        {...formController.getInputProps('employeeId')} />

      <Group mt="md" grow>
        <TextInput
          required
          label="Last Name"
          key={formController.key('lastName')}
          {...formController.getInputProps('lastName')} />

        <TextInput
          required
          label="First Name"
          key={formController.key('firstName')}
          {...formController.getInputProps('firstName')} />

        <TextInput
          required
          label="Middle Name"
          key={formController.key('middleName')}
          {...formController.getInputProps('middleName')} />
      </Group>

      <Group mt="md" grow>
        <TextInput
          required
          label="Street"
          key={formController.key('address.street')}
          {...formController.getInputProps('address.street')} />

        <TextInput
          required
          label="City"
          key={formController.key('address.city')}
          {...formController.getInputProps('address.city')} />

        <TextInput
          required
          label="Province"
          key={formController.key('address.province')}
          {...formController.getInputProps('address.province')} />
      </Group>

      <Group mt="md" grow>
        <TextInput
          label="Landline number"
          key={formController.key('address.landlineNumber')}
          {...formController.getInputProps('address.landlineNumber')} />

        <TextInput
          label="Mobile number"
          key={formController.key('address.mobileNumber')}
          {...formController.getInputProps('address.mobileNumber')} />
      </Group>

      <Group mt="md" grow>
        <TextInput
          label="TIN"
          key={formController.key('tin')}
          {...formController.getInputProps('tin')} />

        <TextInput
          label="Pag-ibig ID"
          key={formController.key('pagIbigId')}
          {...formController.getInputProps('pagIbigId')} />

        <TextInput
          label="Philhealth ID"
          key={formController.key('philhealthId')}
          {...formController.getInputProps('philhealthId')} />
      </Group>

      <Group mt="md" grow>
        <NumberInput
          label="Rate"
          key={formController.key('rate')}
          {...formController.getInputProps('rate')} />

        <Select
          required
          label="Salary Unit"
          searchable
          checkIconPosition="right"
          data={salaryUnits.map(e => ({
            value: e,
            label: e
          }))}
          key={formController.key('salaryUnit')}
          {...formController.getInputProps('salaryUnit')} />
      </Group>

      <Group justify="flex-end" mt="md">
        <Button type="submit" loading={formController.submitting}>Save</Button>
      </Group>
    </form>
  )
}