import { useMutation, useQuery } from "@tanstack/react-query";
import axios from "axios";

export function usePostPayment() {
  return useMutation({
    mutationFn: async (request) => {
      const result = await axios.post('/api/Payments', request)
      return result.data
    }
  })
}

export function useGetPayments() {
  return useQuery({
    queryKey: [ 'disbursements' ],
    queryFn: async () => {
      const result = await axios.get('/api/Payments')
      return result.data
    },
    staleTime: 300000
  })
}