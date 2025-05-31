import { useMutation } from "@tanstack/react-query";
import axios from "axios";

export function usePostPayment() {
  return useMutation({
    mutationFn: async (request) => {
      const result = await axios.post('/api/Payments', request)
      return result.data
    }
  })
}