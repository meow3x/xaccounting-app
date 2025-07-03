import CrudToolbar from "src/Shared/CrudToolbar";
import DisbursementForm from "./DisbursementForm";
import DisbursementsList from "./DIsbursementsList";

export default function DisbursementIndex() {
  return (
    <>
      <CrudToolbar title="Disbursements" />
      {/* <DisbursementForm /> */}
      <DisbursementsList />
    </>
  )
}