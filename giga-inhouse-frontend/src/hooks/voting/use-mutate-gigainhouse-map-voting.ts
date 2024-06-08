import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useMutation, useQueryClient } from "@tanstack/react-query";

type MutateVoteOptions = {
  lobbyId: string;
  mapName: string;
};

export function useMutateGigaInhouseVoting() {
  const api = useGigaInhouseApi();
  const queryClient = useQueryClient();

  const vote = useMutation({
    mutationFn: async ({ lobbyId, mapName }: MutateVoteOptions) => {
      await api.request({
        url: "/Lobby/Vote",
        method: "POST",
        params: {
          lobbyId: lobbyId,
          mapName: mapName,
        },
      });
    },
    onSettled: (_d, _e, { lobbyId }) => {
      queryClient.invalidateQueries({
        queryKey: ["giga-inhouse", "voting", lobbyId],
      });
    },
  });

  return vote;
}
