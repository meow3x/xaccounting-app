import CrudToolbar from "src/Shared/CrudToolbar";
import DisbursementForm from "./DisbursementForm";

export default function DisbursementIndex() {
  return (
    <>
      <CrudToolbar title="Disbursements" />
      <DisbursementForm />
    </>
  )
}