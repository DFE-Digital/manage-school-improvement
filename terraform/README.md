This documentation covers the deployment of the infrastructure to host the app.

## Azure infrastructure

The infrastructure is managed using [Terraform](https://www.terraform.io/).<br>
The state is stored remotely in encrypted Azure storage.<br>
[Terraform workspaces](https://www.terraform.io/docs/state/workspaces.html) are used to separate environments.

#### Configuring the storage backend

The Terraform state is stored remotely in Azure, this allows multiple team members to
make changes and means the state file is backed up. The state file contains
sensitive information so access to it should be restricted, and it should be stored
encrypted at rest.

##### Create a new storage backend

This step only needs to be done once per project (eg. not per environment).
If it has already been created, obtain the storage backend attributes and skip to the next step.

The [Azure tutorial](https://docs.microsoft.com/en-us/azure/developer/terraform/store-state-in-azure-storage) outlines the steps to create a storage account and container for the state file. You will need:

- resource_group_name: The name of the resource group used for the Azure Storage account.
- storage_account_name: The name of the Azure Storage account.
- container_name: The name of the blob container.
- key: The name of the state store file to be created.

##### Create a backend configuration file

Create a new file named `backend.vars` with the following content:

```
resource_group_name  = [the name of the Azure resource group]
storage_account_name = [the name of the Azure Storage account]
container_name       = [the name of the blob container]
key                  = "terraform.tstate"
```

##### Install dependencies

We can use [Homebrew](https://brew.sh) to install the dependecies we need to deploy the infrastructure (eg. tfenv, Azure cli).
These are listed in the `Brewfile`

to install, run:

```
$ brew bundle
```

##### Log into azure with the Azure CLI

Log in to your account:

```
$ az login
```

Confirm which account you are currently using:

```
$ az account show
```

To list the available subscriptions, run:

```
$ az account list
```

Then if needed, switch to it using the 'id':

```
$ az account set --subscription <id>
```

##### Initialise Terraform

Install the required terraform version with the Terraform version manager `tfenv`:

```
$ tfenv install
```

Initialize Terraform to download the required Terraform modules and configure the remote state backend
to use the settings you specified in the previous step.

`$ terraform init -backend-config=backend.vars`

##### Create a Terraform variables file

Each environment will need it's own `tfvars` file.

Copy the `terraform.tfvars.example` to `environment-name.tfvars` and modify the contents as required

##### Create the infrastructure

Now Terraform has been initialised you can create a workspace if needed:

`$ terraform workspace new staging`

Or to check what workspaces already exist:

`$ terraform workspace list`

Switch to the new or existing workspace:

`$ terraform workspace select staging`

Plan the changes:

`$ terraform plan -var-file=staging.tfvars`

Terraform will ask you to provide any variables not specified in an `*.auto.tfvars` file.
Now you can run:

`$ terraform apply -var-file=staging.tfvars`

If everything looks good, answer `yes` and wait for the new infrastructure to be created.

##### Azure resources

<!-- BEGIN_TF_DOCS -->
## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_terraform"></a> [terraform](#requirement\_terraform) | ~> 1.9 |
| <a name="requirement_azapi"></a> [azapi](#requirement\_azapi) | ~> 1.13 |
| <a name="requirement_azurerm"></a> [azurerm](#requirement\_azurerm) | ~> 4.0 |
| <a name="requirement_statuscake"></a> [statuscake](#requirement\_statuscake) | ~> 2.1 |

## Providers

No providers.

## Modules

| Name | Source | Version |
|------|--------|---------|
| <a name="module_azure_container_apps_hosting"></a> [azure\_container\_apps\_hosting](#module\_azure\_container\_apps\_hosting) | github.com/DFE-Digital/terraform-azurerm-container-apps-hosting | v1.19.1 |
| <a name="module_azurerm_key_vault"></a> [azurerm\_key\_vault](#module\_azurerm\_key\_vault) | github.com/DFE-Digital/terraform-azurerm-key-vault-tfvars | v0.5.1 |
| <a name="module_data_protection"></a> [data\_protection](#module\_data\_protection) | github.com/DFE-Digital/terraform-azurerm-aspnet-data-protection | v1.3.0 |
| <a name="module_statuscake-tls-monitor"></a> [statuscake-tls-monitor](#module\_statuscake-tls-monitor) | github.com/dfe-digital/terraform-statuscake-tls-monitor | v0.1.5 |

## Resources

No resources.

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_azure_client_id"></a> [azure\_client\_id](#input\_azure\_client\_id) | Service Principal Client ID | `string` | n/a | yes |
| <a name="input_azure_client_secret"></a> [azure\_client\_secret](#input\_azure\_client\_secret) | Service Principal Client Secret | `string` | n/a | yes |
| <a name="input_azure_location"></a> [azure\_location](#input\_azure\_location) | Azure location in which to launch resources. | `string` | n/a | yes |
| <a name="input_azure_subscription_id"></a> [azure\_subscription\_id](#input\_azure\_subscription\_id) | Service Principal Subscription ID | `string` | n/a | yes |
| <a name="input_azure_tenant_id"></a> [azure\_tenant\_id](#input\_azure\_tenant\_id) | Service Principal Tenant ID | `string` | n/a | yes |
| <a name="input_cdn_frontdoor_custom_domains"></a> [cdn\_frontdoor\_custom\_domains](#input\_cdn\_frontdoor\_custom\_domains) | Azure CDN Front Door custom domains. If they are within the DNS zone (optionally created), the Validation TXT records and ALIAS/CNAME records will be created | `list(string)` | `[]` | no |
| <a name="input_cdn_frontdoor_enable_rate_limiting"></a> [cdn\_frontdoor\_enable\_rate\_limiting](#input\_cdn\_frontdoor\_enable\_rate\_limiting) | Enable CDN Front Door Rate Limiting. This will create a WAF policy, and CDN security policy. For pricing reasons, there will only be one WAF policy created. | `bool` | `false` | no |
| <a name="input_cdn_frontdoor_forwarding_protocol"></a> [cdn\_frontdoor\_forwarding\_protocol](#input\_cdn\_frontdoor\_forwarding\_protocol) | Azure CDN Front Door forwarding protocol | `string` | `"HttpsOnly"` | no |
| <a name="input_cdn_frontdoor_health_probe_path"></a> [cdn\_frontdoor\_health\_probe\_path](#input\_cdn\_frontdoor\_health\_probe\_path) | Specifies the path relative to the origin that is used to determine the health of the origin. | `string` | `"/"` | no |
| <a name="input_cdn_frontdoor_health_probe_protocol"></a> [cdn\_frontdoor\_health\_probe\_protocol](#input\_cdn\_frontdoor\_health\_probe\_protocol) | Use Http or Https | `string` | `"Https"` | no |
| <a name="input_cdn_frontdoor_host_add_response_headers"></a> [cdn\_frontdoor\_host\_add\_response\_headers](#input\_cdn\_frontdoor\_host\_add\_response\_headers) | List of response headers to add at the CDN Front Door `[{ "Name" = "Strict-Transport-Security", "value" = "max-age=31536000" }]` | `list(map(string))` | `[]` | no |
| <a name="input_cdn_frontdoor_host_redirects"></a> [cdn\_frontdoor\_host\_redirects](#input\_cdn\_frontdoor\_host\_redirects) | CDN FrontDoor host redirects `[{ "from" = "example.com", "to" = "www.example.com" }]` | `list(map(string))` | `[]` | no |
| <a name="input_cdn_frontdoor_origin_fqdn_override"></a> [cdn\_frontdoor\_origin\_fqdn\_override](#input\_cdn\_frontdoor\_origin\_fqdn\_override) | Manually specify the hostname that the CDN Front Door should target. Defaults to the Container App FQDN | `string` | `""` | no |
| <a name="input_cdn_frontdoor_origin_host_header_override"></a> [cdn\_frontdoor\_origin\_host\_header\_override](#input\_cdn\_frontdoor\_origin\_host\_header\_override) | Manually specify the host header that the CDN sends to the target. Defaults to the recieved host header. Set to null to set it to the host\_name (`cdn_frontdoor_origin_fqdn_override`) | `string` | `""` | no |
| <a name="input_cdn_frontdoor_rate_limiting_duration_in_minutes"></a> [cdn\_frontdoor\_rate\_limiting\_duration\_in\_minutes](#input\_cdn\_frontdoor\_rate\_limiting\_duration\_in\_minutes) | CDN Front Door rate limiting duration in minutes | `number` | `5` | no |
| <a name="input_cdn_frontdoor_rate_limiting_threshold"></a> [cdn\_frontdoor\_rate\_limiting\_threshold](#input\_cdn\_frontdoor\_rate\_limiting\_threshold) | Maximum number of concurrent requests before rate limiting is applied | `number` | `200` | no |
| <a name="input_cdn_frontdoor_vdp_destination_hostname"></a> [cdn\_frontdoor\_vdp\_destination\_hostname](#input\_cdn\_frontdoor\_vdp\_destination\_hostname) | Requires 'enable\_cdn\_frontdoor\_vdp\_redirects' to be set to 'true'. Hostname to redirect security.txt and thanks.txt to | `string` | `"vdp.security.education.gov.uk"` | no |
| <a name="input_cdn_frontdoor_waf_custom_rules"></a> [cdn\_frontdoor\_waf\_custom\_rules](#input\_cdn\_frontdoor\_waf\_custom\_rules) | Map of all Custom rules you want to apply to the CDN WAF | <pre>map(object({<br/>    priority : number,<br/>    action : string<br/>    match_conditions : map(object({<br/>      match_variable : string,<br/>      match_values : optional(list(string), []),<br/>      operator : optional(string, "Any"),<br/>      selector : optional(string, null),<br/>      negation_condition : optional(bool, false),<br/>    }))<br/>  }))</pre> | `{}` | no |
| <a name="input_container_apps_allow_ips_inbound"></a> [container\_apps\_allow\_ips\_inbound](#input\_container\_apps\_allow\_ips\_inbound) | Restricts access to the Container Apps by creating a network security group rule that only allow inbound traffic from the provided list of IPs | `list(string)` | `[]` | no |
| <a name="input_container_command"></a> [container\_command](#input\_container\_command) | Container command | `list(any)` | n/a | yes |
| <a name="input_container_health_probe_path"></a> [container\_health\_probe\_path](#input\_container\_health\_probe\_path) | Specifies the path that is used to determine the liveness of the Container | `string` | n/a | yes |
| <a name="input_container_max_replicas"></a> [container\_max\_replicas](#input\_container\_max\_replicas) | Container max replicas | `number` | n/a | yes |
| <a name="input_container_port"></a> [container\_port](#input\_container\_port) | Container port | `number` | `8080` | no |
| <a name="input_container_scale_http_concurrency"></a> [container\_scale\_http\_concurrency](#input\_container\_scale\_http\_concurrency) | When the number of concurrent HTTP requests exceeds this value, then another replica is added. Replicas continue to add to the pool up to the max-replicas amount. | `number` | `10` | no |
| <a name="input_container_secret_environment_variables"></a> [container\_secret\_environment\_variables](#input\_container\_secret\_environment\_variables) | Container secret environment variables | `map(string)` | n/a | yes |
| <a name="input_dns_alias_records"></a> [dns\_alias\_records](#input\_dns\_alias\_records) | DNS ALIAS records to add to the DNS Zone | <pre>map(<br/>    object({<br/>      ttl : optional(number, 300),<br/>      target_resource_id : string<br/>    })<br/>  )</pre> | `{}` | no |
| <a name="input_dns_mx_records"></a> [dns\_mx\_records](#input\_dns\_mx\_records) | DNS MX records to add to the DNS Zone | <pre>map(<br/>    object({<br/>      ttl : optional(number, 300),<br/>      records : list(<br/>        object({<br/>          preference : number,<br/>          exchange : string<br/>        })<br/>      )<br/>    })<br/>  )</pre> | `{}` | no |
| <a name="input_dns_ns_records"></a> [dns\_ns\_records](#input\_dns\_ns\_records) | DNS NS records to add to the DNS Zone | <pre>map(<br/>    object({<br/>      ttl : optional(number, 300),<br/>      records : list(string)<br/>    })<br/>  )</pre> | n/a | yes |
| <a name="input_dns_txt_records"></a> [dns\_txt\_records](#input\_dns\_txt\_records) | DNS TXT records to add to the DNS Zone | <pre>map(<br/>    object({<br/>      ttl : optional(number, 300),<br/>      records : list(string)<br/>    })<br/>  )</pre> | n/a | yes |
| <a name="input_dns_zone_domain_name"></a> [dns\_zone\_domain\_name](#input\_dns\_zone\_domain\_name) | DNS zone domain name. If created, records will automatically be created to point to the CDN. | `string` | n/a | yes |
| <a name="input_enable_cdn_frontdoor"></a> [enable\_cdn\_frontdoor](#input\_enable\_cdn\_frontdoor) | Enable Azure CDN Front Door. This will use the Container Apps endpoint as the origin. | `bool` | `false` | no |
| <a name="input_enable_cdn_frontdoor_health_probe"></a> [enable\_cdn\_frontdoor\_health\_probe](#input\_enable\_cdn\_frontdoor\_health\_probe) | Enable CDN Front Door health probe | `bool` | `false` | no |
| <a name="input_enable_cdn_frontdoor_vdp_redirects"></a> [enable\_cdn\_frontdoor\_vdp\_redirects](#input\_enable\_cdn\_frontdoor\_vdp\_redirects) | Deploy redirects for security.txt and thanks.txt to an external Vulnerability Disclosure Program service | `bool` | `false` | no |
| <a name="input_enable_container_app_file_share"></a> [enable\_container\_app\_file\_share](#input\_enable\_container\_app\_file\_share) | Create an Azure Storage Account and File Share to be mounted to the Container Apps | `bool` | `false` | no |
| <a name="input_enable_container_registry"></a> [enable\_container\_registry](#input\_enable\_container\_registry) | Set to true to create a container registry | `bool` | n/a | yes |
| <a name="input_enable_dns_zone"></a> [enable\_dns\_zone](#input\_enable\_dns\_zone) | Conditionally create a DNS zone | `bool` | n/a | yes |
| <a name="input_enable_event_hub"></a> [enable\_event\_hub](#input\_enable\_event\_hub) | Send Azure Container App logs to an Event Hub sink | `bool` | `false` | no |
| <a name="input_enable_health_insights_api"></a> [enable\_health\_insights\_api](#input\_enable\_health\_insights\_api) | Deploys a Function App that exposes the last 3 HTTP Web Tests via an API endpoint. 'enable\_app\_insights\_integration' and 'enable\_monitoring' must be set to 'true'. | `bool` | `false` | no |
| <a name="input_enable_init_container"></a> [enable\_init\_container](#input\_enable\_init\_container) | Deploy an Init Container. Init containers run before the primary app container and are used to perform initialization tasks such as downloading data or preparing the environment | `bool` | `false` | no |
| <a name="input_enable_logstash_consumer"></a> [enable\_logstash\_consumer](#input\_enable\_logstash\_consumer) | Create an Event Hub consumer group for Logstash | `bool` | `false` | no |
| <a name="input_enable_monitoring"></a> [enable\_monitoring](#input\_enable\_monitoring) | Create an App Insights instance and notification group for the Container App | `bool` | n/a | yes |
| <a name="input_enable_monitoring_traces"></a> [enable\_monitoring\_traces](#input\_enable\_monitoring\_traces) | Monitor App Insights traces for error messages | `bool` | `true` | no |
| <a name="input_enable_mssql_database"></a> [enable\_mssql\_database](#input\_enable\_mssql\_database) | Set to true to create an Azure SQL server/database, with a private endpoint within the virtual network | `bool` | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | Environment name. Will be used along with `project_name` as a prefix for all resources. | `string` | n/a | yes |
| <a name="input_eventhub_export_log_analytics_table_names"></a> [eventhub\_export\_log\_analytics\_table\_names](#input\_eventhub\_export\_log\_analytics\_table\_names) | List of Log Analytics table names that you want to export to Event Hub. See https://learn.microsoft.com/en-gb/azure/azure-monitor/logs/logs-data-export?tabs=portal#supported-tables for a list of supported tables | `list(string)` | `[]` | no |
| <a name="input_existing_logic_app_workflow"></a> [existing\_logic\_app\_workflow](#input\_existing\_logic\_app\_workflow) | Name, and Resource Group of an existing Logic App Workflow. Leave empty to create a new Resource | <pre>object({<br/>    name : string<br/>    resource_group_name : string<br/>  })</pre> | <pre>{<br/>  "name": "",<br/>  "resource_group_name": ""<br/>}</pre> | no |
| <a name="input_existing_network_watcher_name"></a> [existing\_network\_watcher\_name](#input\_existing\_network\_watcher\_name) | Use an existing network watcher to add flow logs. | `string` | n/a | yes |
| <a name="input_existing_network_watcher_resource_group_name"></a> [existing\_network\_watcher\_resource\_group\_name](#input\_existing\_network\_watcher\_resource\_group\_name) | Existing network watcher resource group. | `string` | n/a | yes |
| <a name="input_health_insights_api_cors_origins"></a> [health\_insights\_api\_cors\_origins](#input\_health\_insights\_api\_cors\_origins) | List of hostnames that are permitted to contact the Health insights API | `list(string)` | <pre>[<br/>  "*"<br/>]</pre> | no |
| <a name="input_health_insights_api_ipv4_allow_list"></a> [health\_insights\_api\_ipv4\_allow\_list](#input\_health\_insights\_api\_ipv4\_allow\_list) | List of IPv4 addresses that are permitted to contact the Health insights API | `list(string)` | `[]` | no |
| <a name="input_image_name"></a> [image\_name](#input\_image\_name) | Image name | `string` | n/a | yes |
| <a name="input_init_container_command"></a> [init\_container\_command](#input\_init\_container\_command) | Container command for the Init Container | `list(any)` | `[]` | no |
| <a name="input_init_container_image"></a> [init\_container\_image](#input\_init\_container\_image) | Image name for the Init Container. Leave blank to use the same Container image from the primary app | `string` | `""` | no |
| <a name="input_key_vault_access_ipv4"></a> [key\_vault\_access\_ipv4](#input\_key\_vault\_access\_ipv4) | List of IPv4 Addresses that are permitted to access the Key Vault | `list(string)` | n/a | yes |
| <a name="input_monitor_email_receivers"></a> [monitor\_email\_receivers](#input\_monitor\_email\_receivers) | A list of email addresses that should be notified by monitoring alerts | `list(string)` | n/a | yes |
| <a name="input_monitor_endpoint_healthcheck"></a> [monitor\_endpoint\_healthcheck](#input\_monitor\_endpoint\_healthcheck) | Specify a route that should be monitored for a 200 OK status | `string` | n/a | yes |
| <a name="input_monitor_http_availability_fqdn"></a> [monitor\_http\_availability\_fqdn](#input\_monitor\_http\_availability\_fqdn) | Specify a FQDN to monitor for HTTP Availability. Leave unset to dynamically calculate the correct FQDN | `string` | `""` | no |
| <a name="input_mssql_azuread_admin_object_id"></a> [mssql\_azuread\_admin\_object\_id](#input\_mssql\_azuread\_admin\_object\_id) | Object ID of a User within Azure AD that you want to assign as the SQL Server Administrator | `string` | n/a | yes |
| <a name="input_mssql_azuread_admin_username"></a> [mssql\_azuread\_admin\_username](#input\_mssql\_azuread\_admin\_username) | Username of a User within Azure AD that you want to assign as the SQL Server Administrator | `string` | n/a | yes |
| <a name="input_mssql_database_name"></a> [mssql\_database\_name](#input\_mssql\_database\_name) | The name of the MSSQL database to create. Must be set if `enable_mssql_database` is true | `string` | n/a | yes |
| <a name="input_mssql_firewall_ipv4_allow_list"></a> [mssql\_firewall\_ipv4\_allow\_list](#input\_mssql\_firewall\_ipv4\_allow\_list) | A list of IPv4 Addresses that require remote access to the MSSQL Server | <pre>map(object({<br/>    start_ip_range : string,<br/>    end_ip_range : optional(string, "")<br/>  }))</pre> | `{}` | no |
| <a name="input_mssql_managed_identity_assign_role"></a> [mssql\_managed\_identity\_assign\_role](#input\_mssql\_managed\_identity\_assign\_role) | Assign the 'Storage Blob Data Contributor' Role to the SQL Server User-Assigned Managed Identity. Note: If you do not have 'Microsoft.Authorization/roleAssignments/write' permission, you will need to manually assign the 'Storage Blob Data Contributor' Role to the identity | `bool` | `false` | no |
| <a name="input_mssql_server_admin_password"></a> [mssql\_server\_admin\_password](#input\_mssql\_server\_admin\_password) | The local administrator password for the MSSQL server | `string` | n/a | yes |
| <a name="input_mssql_server_public_access_enabled"></a> [mssql\_server\_public\_access\_enabled](#input\_mssql\_server\_public\_access\_enabled) | Enable public internet access to your MSSQL instance. Be sure to specify 'mssql\_firewall\_ipv4\_allow\_list' to restrict inbound connections | `bool` | `false` | no |
| <a name="input_mssql_sku_name"></a> [mssql\_sku\_name](#input\_mssql\_sku\_name) | Specifies the name of the SKU used by the database | `string` | `"Basic"` | no |
| <a name="input_project_name"></a> [project\_name](#input\_project\_name) | Project name. Will be used along with `environment` as a prefix for all resources. | `string` | n/a | yes |
| <a name="input_registry_admin_enabled"></a> [registry\_admin\_enabled](#input\_registry\_admin\_enabled) | Do you want to enable access key based authentication for your Container Registry? | `bool` | `true` | no |
| <a name="input_registry_managed_identity_assign_role"></a> [registry\_managed\_identity\_assign\_role](#input\_registry\_managed\_identity\_assign\_role) | Assign the 'AcrPull' Role to the Container App User-Assigned Managed Identity. Note: If you do not have 'Microsoft.Authorization/roleAssignments/write' permission, you will need to manually assign the 'AcrPull' Role to the identity | `bool` | `false` | no |
| <a name="input_registry_server"></a> [registry\_server](#input\_registry\_server) | Container registry server | `string` | `""` | no |
| <a name="input_registry_use_managed_identity"></a> [registry\_use\_managed\_identity](#input\_registry\_use\_managed\_identity) | Create a User-Assigned Managed Identity for the Container App. Note: If you do not have 'Microsoft.Authorization/roleAssignments/write' permission, you will need to manually assign the 'AcrPull' Role to the identity | `bool` | `true` | no |
| <a name="input_statuscake_api_token"></a> [statuscake\_api\_token](#input\_statuscake\_api\_token) | API token for StatusCake | `string` | `"00000000000000000000000000000"` | no |
| <a name="input_statuscake_contact_group_email_addresses"></a> [statuscake\_contact\_group\_email\_addresses](#input\_statuscake\_contact\_group\_email\_addresses) | List of email address that should receive notifications from StatusCake | `list(string)` | `[]` | no |
| <a name="input_statuscake_contact_group_integrations"></a> [statuscake\_contact\_group\_integrations](#input\_statuscake\_contact\_group\_integrations) | List of Integration IDs to connect to your Contact Group | `list(string)` | `[]` | no |
| <a name="input_statuscake_contact_group_name"></a> [statuscake\_contact\_group\_name](#input\_statuscake\_contact\_group\_name) | Name of the contact group in StatusCake | `string` | `""` | no |
| <a name="input_statuscake_monitored_resource_addresses"></a> [statuscake\_monitored\_resource\_addresses](#input\_statuscake\_monitored\_resource\_addresses) | The URLs to perform TLS checks on | `list(string)` | `[]` | no |
| <a name="input_storage_account_public_access_enabled"></a> [storage\_account\_public\_access\_enabled](#input\_storage\_account\_public\_access\_enabled) | Should the Azure Storage Account have Public visibility? | `bool` | `true` | no |
| <a name="input_tags"></a> [tags](#input\_tags) | Tags to be applied to all resources | `map(string)` | n/a | yes |
| <a name="input_tfvars_filename"></a> [tfvars\_filename](#input\_tfvars\_filename) | tfvars filename. This file is uploaded and stored encrupted within Key Vault, to ensure that the latest tfvars are stored in a shared place. | `string` | n/a | yes |
| <a name="input_virtual_network_address_space"></a> [virtual\_network\_address\_space](#input\_virtual\_network\_address\_space) | Virtual network address space CIDR | `string` | n/a | yes |

## Outputs

No outputs.
<!-- END_TF_DOCS -->
