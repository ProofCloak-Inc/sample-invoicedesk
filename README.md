# InvoiceDesk: a ProofCloak before/after sample

This repository lets you look at what [ProofCloak](https://proofcloak.com/en)
does to a .NET assembly without creating an account. It holds a small sample
app, the protected build that came back from the service, and the evidence
files of that job.

Nothing here is mocked. The protected DLL is the service's output, and the
decompiled listings are plain `ilspycmd` output.

## What is in here

| Path | Content |
| --- | --- |
| `src/` | The sample: a .NET 10 console app with a license check, a pricing rule and a JSON settings class. All types are `internal`. |
| `before/` | Not stored. `verify.ps1` builds the Release DLL here from `src/`. |
| `protected/InvoiceDesk.dll` | The protected output: renaming, string protection, control-flow rewriting and anti-tamper checks. |
| `decompiled/` | `LicenseValidator` and `AppSettings` decompiled from both files. |
| `evidence/` | The before and after inspection reports and an excerpt of the execution receipt. |
| `verify.ps1` | Checks the hashes against the evidence and runs both builds. |

## Check it yourself

You need the .NET 10 SDK and PowerShell 7 (`pwsh`, on Windows, Linux or macOS).

```bash
pwsh ./verify.ps1
```

The script compares the SHA-256 of the protected DLL and of both inspection
reports with the receipt, builds the original from `src/`, runs both builds,
and fails if the two outputs differ.

The uploaded input itself is not in the repository. A Release DLL embeds the
path it was built in, so the DLL you build has a different hash than the one
in the receipt. The script says so and carries on.

To decompile the files yourself:

```bash
dotnet tool install -g ilspycmd
```

```bash
ilspycmd before/InvoiceDesk.dll -t InvoiceDesk.Licensing.LicenseValidator
```

The protected file has no type called `LicenseValidator` any more, so
decompile all of it:

```bash
ilspycmd protected/InvoiceDesk.dll
```

## What changed

| | Input | Protected |
| --- | --- | --- |
| Size | 11,264 bytes | 19,968 bytes |
| Types | 7 | 10 |
| Methods | 40 | 62 |
| Fields | 11 | 25 |
| String literals encrypted | | 17 |
| Methods with rewritten control flow | | 8 |
| Method bodies encrypted | | 0 (not requested) |

Both builds print the same four lines. The third line reads a settings file
"written by an earlier release", so it would change if a JSON property had
been renamed:

```text
License valid: True
Total (HU): 1567.73
Settings file: Contoso Kft, HUF, 45 days
Settings saved: {"CompanyName":"Contoso Kft","DefaultCurrency":"HUF","PaymentTermDays":45}
```

In `decompiled/LicenseValidator.after.cs`:

- The class, both methods and the field have generated names.
- The salt is not in the file any more. The field is filled by a decryptor
  call.
- The branch in `IsValid` is a state machine driven by an opaque integer.

## What did not change

- Framework calls are visible. You can still see a SHA-256 hash and a
  fixed-time comparison. This profile doesn't hide call targets.
- Parameter names are still there.
- In `AppSettings` the public properties kept their names. That is deliberate:
  public properties are treated as a possible data contract, and it is why
  the settings file still loads. The class name and the backing fields were
  renamed.
- A `const` string would stay readable. The compiler stores it as a literal
  in metadata, and this profile doesn't encrypt constant fields. The sample
  uses `static readonly` for that reason.

Obfuscation raises the cost of reading and patching a binary. It does not make
a secret in client code safe.

## The sample is not a product

`LicenseValidator` is demo logic, not a licensing scheme. Do not copy it.

## Licence

The sample source in `src/` and `verify.ps1` are under the MIT licence. The
files in `protected/`, `decompiled/` and `evidence/` are output of the
ProofCloak service and are here for inspection and verification only. See
[LICENSE](LICENSE).

## Try it on your own build

ProofCloak is free during the open beta: one project and five protection jobs
a month. It targets .NET 10 managed DLL and EXE files.

A new profile starts with only "Rename internal symbols" ticked. To get the
same result as this sample, also tick these three when you create your first
profile:

- Encrypt strings
- Control-flow transformation
- Anti-tamper checks

[Protect your first .NET build free](https://proofcloak.com/en?utm_source=github&utm_medium=referral&utm_campaign=launch-v2&utm_content=sample-invoicedesk)
