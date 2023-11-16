{
  description = "Flake for the game WorldOfZuul";

  outputs = { self, nixpkgs }: 
  let 
    systems = [ "x86_64-linux" "x86_64-darwin" ];
  in
  {
    packages = nixpkgs.lib.genAttrs systems (system: 
      let
        pkgs = nixpkgs.legacyPackages.${system};
      in
      {
        default = pkgs.buildDotnetModule rec {
          pname = "WorldOfZuul";
          version = "0.1";
          src = ./.;

          projectFile = "WorldOfZuul.csproj";
          dotnet-sdk = pkgs.dotnetCorePackages.sdk_7_0;
          dotnet-runtime = pkgs.dotnetCorePackages.runtime_7_0;

          nugetDeps = ./deps.nix;


        };

        }

     );
  };
}