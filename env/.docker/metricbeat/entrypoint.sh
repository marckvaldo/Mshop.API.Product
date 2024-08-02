#!/bin/sh
# Ajustar permissões do arquivo de configuração
chmod go-w /usr/share/metricbeat/metricbeat.yml
# Iniciar o Metricbeat
exec metricbeat -e
