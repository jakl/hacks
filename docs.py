#!/usr/bin/python
import gdata.docs.service
import getpass

#create client class to make web requiests to doc server
client = gdata.docs.service.DocsService()

try:#to use an auth token from a previous session
  f= open('./token','r')
  token = f.readline()
  client.SetClientLoginToken(token)
except:
  try:#to aquire a new auth token to use
    email = raw_input("Gmail:")
    password = getpass.unix_getpass("Pass:")

    #Auth using docs name/pass
    client.ClientLogin(email,password)

  #Sometimes captchas are required
  except gdata.service.CaptchaRequired:
    print "Solve captcha @"
    print client.captcha_url
    answer = raw_input("Captcha answer:")

    #Requthenicate after solving the captcha
    client.ClientLogin(email,password,captcha_token=client.captcha_token,captcha_response=answer)

  except gdata.service.BadAuthentication:
    exit("Credentials unrecognized")
  except gdata.service.Error:
    exit("Login error")

  #write the new auth token to a local file
  f = open('./token', 'w')
  f.write(client.GetClientLoginToken())

#query server for Atom feed of documents
documents_feed = client.GetDocumentListFeed()
#Loop through feed, extracting document entries
for document_entry in documents_feed.entry:
  #display titles of every doc on user account
  print document_entry.title.text
