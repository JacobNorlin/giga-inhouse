import { LobbyPlayer } from "@giga-inhouse/components/lobby/lobby-player";
import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import {
  MapVoting as MapVotingType,
  useGigaInhouseVoting,
} from "@giga-inhouse/hooks/voting/use-giga-inhouse-map-voting";
import { useMutateGigaInhouseVoting } from "@giga-inhouse/hooks/voting/use-mutate-gigainhouse-map-voting";
import { Card, Flex, Group, Stack, Text } from "@mantine/core";

type MapVotingProps = {
  lobbyId: string;
};

function getAnnoatedMaps(mapVoting: MapVotingType) {
  // Annotate each map with ban flag and the users who have voted for it
  const annotatedMaps = mapVoting.maps.map((map) => {
    const isBanned = mapVoting.bannedMaps.includes(map.name);

    const matchingVotes = mapVoting.votes.filter((vote) => {
      return vote.mapName === map.name;
    });

    return {
      ...map,
      isBanned: isBanned,
      votes: matchingVotes,
    };
  });

  return annotatedMaps;
}

export function MapVoting({ lobbyId }: MapVotingProps) {
  const votingQuery = useGigaInhouseVoting(lobbyId);
  const vote = useMutateGigaInhouseVoting();

  return (
    <Flex wrap="wrap" gap={2}>
      <QueryWrapper queryStates={[votingQuery]}>
        {(mapVoting) => {
          const annotatedMaps = getAnnoatedMaps(mapVoting);
          return annotatedMaps.map((map) => {
            return (
              <Card
                onClick={() => {
                  vote.mutate({ lobbyId: lobbyId, mapName: map.name });
                }}
              >
                <Card.Section>
                  <Text size="lg">{map.name}</Text>
                </Card.Section>

                <Stack>
                  {map.votes.map((vote) => {
                    return <LobbyPlayer user={vote.user} />;
                  })}
                </Stack>
              </Card>
            );
          });
        }}
      </QueryWrapper>
    </Flex>
  );
}
