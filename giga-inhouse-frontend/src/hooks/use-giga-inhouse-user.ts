import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";
import { useNavigate } from "react-router";

export function useGigaInhouseUser() {
  const api = useGigaInhouseApi();
  const navigate = useNavigate();

  const userQuery = useQuery({
    queryKey: ["giga-inhouse", "user"],
    queryFn: async () => {
      const res = await api.request({
        url: "/User",
        method: "GET",
      });

      return res.data;
    },
    retryOnMount: true,
    retry: false,
  });

  const queryError = userQuery.error as AxiosError;
  if (queryError) {
    navigate("/login");
  }

  return userQuery;
}
