import { useMutation, useQuery } from "@tanstack/react-query";
import axios from "axios";

export function useCreateApVoucher() {
  return useMutation({
    mutationFn: async (e) => {
      const result = await axios.post('/api/AccountsPayable', e)
      return result.data
    }
  })
}

export function useGetAccountsPayable() {
  return useQuery({
    queryKey: ['accounts-payable'],
    queryFn: async() => {
      const result = await axios.get('/api/AccountsPayable')
      return result.data
    },
    staleTime: 300000
  })
}

export function useGetVouchers() {
  return useQuery({
    queryKey: ['ap-vouchers'],
    queryFn: async () => {
      const result = await axios.get('/api/AccountsPayable/Vouchers')
      return result.data
    },
    staleTime: 300000
  })
}

export function useGetPayable(voucherNumber) {
  return useQuery({
    queryKey: ['accounts-payable', voucherNumber],
    queryFn: async () => {
      if (voucherNumber) {
        const result = await axios.get(`/api/AccountsPayable/${voucherNumber}`)
        return result.data
      }
      return null
    },
    staleTime: 300000
  })
}

export function useGetPayableBalance(supplierId) {
  return useQuery({
    queryKey: ['payable-balance', supplierId],
    queryFn: async() => {
      if (supplierId) {
        const result = await axios.get(`/api/AccountsPayable/Balance/Supplier/${supplierId}`)
        return result.data
      }

      return null
    }
  })
}