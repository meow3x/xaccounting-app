import {useForm} from "@mantine/form";
import {Modal} from "@mantine/core";
import {employeeRepository} from "src/Employees/api.js";
import {EmployeeForm, makeEmployeeForm} from "src/Employees/EmployeeForm.jsx";

export default function CreateEmployeeModal({ opened, onClose, onDataCreated, salaryUnitsRef }) {
  const form = useForm(makeEmployeeForm())

  function handleSubmit(formData) {
    employeeRepository.create(formData).then(e => {
      form.reset()
      onDataCreated?.(e)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create employee" size="xl">
      <EmployeeForm
        formController={form}
        handleSubmit={handleSubmit}
        salaryUnits={salaryUnitsRef} />
    </Modal>
  )
}