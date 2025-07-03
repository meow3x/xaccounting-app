import { useMutation } from "@tanstack/react-query";
import axios from "axios";

export function useCreateInvoice() {
  return useMutation({
    mutationFn: async (e) => {
      const result = await axios.post('/api/Invoice', e)
      return result.data
    }
  })
}