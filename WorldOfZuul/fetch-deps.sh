echo building fetch-deps
nix build .#default.fetch-deps
echo running result
./result ./deps.nix
