import { UserInfo } from "@giga-inhouse/components/auth-wrapper/auth-context";
import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useQuery } from "@tanstack/react-query";

type GigaInhouseLobby = {
  users: UserInfo[];
};

export function useGigaInhouseLobby() {
  const api = useGigaInhouseApi();

  const query = useQuery({
    queryKey: ["giga-inhouse", "lobby"],
    queryFn: async () => {
      const res = await api.request<GigaInhouseLobby>({
        url: "Lobby",
        method: "GET",
      });

      return res.data;
    },
  });

  return query;
}
