import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export function useCreateGigaInhouseLobby() {
  const queryClient = useQueryClient();
  const api = useGigaInhouseApi();
  const navigate = useNavigate();

  const createLobby = useMutation({
    mutationFn: async () => {
      const res = await api.request<string>({
        url: "/Lobby/Create",
        method: "POST",
      });

      const lobbyId = res.data;

      if (!lobbyId) {
        return;
      }

      await api.request({
        url: "/Lobby/Join",
        method: "POST",
        params: {
          lobbyId,
        },
      });

      return res.data;
    },
    onSettled: (lobbyId) => {
      if (!lobbyId) {
        return;
      }

      queryClient.invalidateQueries({ queryKey: ["giga-inhouse", "lobbies"] });
      navigate(`/lobby/${lobbyId}`);
    },
  });

  return createLobby;
}
